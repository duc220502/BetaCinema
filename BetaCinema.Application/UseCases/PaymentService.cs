using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.Interfaces.PaymentStrategies;
using BetaCinema.Application.UseCases.Users;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class PaymentService(IPaymentStrategyFactory strategyFactory, IUnitOfWork unitOfWork,
        IBillRepository billRepository, IPromotionService promotionService,
        IUserService userService, ISeatService seatService,
        IScheduleRepository scheduleRepository , 
        IPaymentRepository paymentRepository , 
        IConfiguration configuration , IBackgroundJobService backgroundJobService) : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPaymentStrategyFactory _strategyFactory = strategyFactory;
        private readonly IBillRepository _billRepository = billRepository;
        private readonly IPromotionService _promotionService = promotionService;
        private readonly IUserService _userService = userService;
        private readonly ISeatService _seatService = seatService;
        private readonly IScheduleRepository _scheduleRepository = scheduleRepository;
        private readonly IPaymentRepository _paymentRepository = paymentRepository;
        private readonly IConfiguration _configuration = configuration;
        private readonly IBackgroundJobService _backgroundJobService = backgroundJobService;

        public async Task<ResponseObject<PaymentInitiationResult>> InitiatePaymentAsync(Guid billId, Domain.Enums.PaymentMethod paymentMethod)
        {
            var bill = await _billRepository.GetBillWithDetailsAsync(billId)
            ?? throw new NotFoundException("Không tìm thấy hóa đơn.");


            if (bill.BillStatusId != (int)Domain.Enums.BillStatus.PendingPayment && bill.BillStatusId != (int)Domain.Enums.BillStatus.Failed)
                throw new InvalidOperationException("Hóa đơn không ở trạng thái chờ thanh toán hoặc thanh toán thất bại  ");


            var paymentMethodEntity = await _paymentRepository.GetPaymentByIdAsync((int)paymentMethod)
                ?? throw new InvalidOperationException("Hình thức thanh toán không hợp lệ.");


            var schedule = await _scheduleRepository.GetScheduleByBillIdAsync(billId)
             ?? throw new InvalidOperationException("Không thể xác định suất chiếu cho hóa đơn này.");

            var cutofftimedefault = _configuration.GetValue<int>("BookingSettings:DefaultHoldMinutes");

            DateTime? newExpirationTime = null;

            if (paymentMethodEntity.ExpirationTimeInMinutes.HasValue)
            {

                var expirationFromMethod = DateTime.UtcNow.AddMinutes(paymentMethodEntity.ExpirationTimeInMinutes.Value);

                var cutOffTime = schedule.StartAt.AddMinutes(cutofftimedefault);

                newExpirationTime = (cutOffTime < expirationFromMethod) ? cutOffTime : expirationFromMethod;
            }

            bill.PaymentAttemptCount++;
            bill.PaymentMethodId = (int)paymentMethod;


            if (newExpirationTime.HasValue)
            {
                bill.ExpireAt = newExpirationTime.Value;
            }

           _billRepository.Update(bill);

            var strategy = _strategyFactory.GetStrategy(paymentMethod);
            var paymentInitiationResult = await strategy.InitiatePaymentAsync(bill);

            await _unitOfWork.SaveChangesAsync();
            return ResponseObject<PaymentInitiationResult>.ResponseSuccess("Khởi tạo yêu cầu thành toán thành công" , paymentInitiationResult); 

        }

        public async Task<ResponseObject<object>> ProcessVnpayIpnAsync(Dictionary<string, string> vnpayResponseData)
        {
            var vnpayStrategy = _strategyFactory.GetStrategy(Domain.Enums.PaymentMethod.VNPAY);
            var confirmationResult = await vnpayStrategy.ConfirmPaymentAsync(vnpayResponseData);

            var bill = await _billRepository.GetBillDetailsForResponseAsync(confirmationResult.BillId)
           ?? throw new NotFoundException("Hóa đơn trong IPN không tồn tại.");

            if (!confirmationResult.IsSuccess)
            {
                bill.BillStatusId =  (int)Domain.Enums.BillStatus.Failed;
                await _unitOfWork.SaveChangesAsync();
                throw new BadRequestException($"Xác thực VNPAY IPN thất bại cho hóa đơn {bill.Id}: {confirmationResult.ErrorMessage}");
            }           

            if (bill.TotalMoney != confirmationResult.Amount)
            {
                bill.BillStatusId = (int)Domain.Enums.BillStatus.ReconciliationFailed;
                await _unitOfWork.SaveChangesAsync();

                var alertViewModel = new ReconciliationFailedAlertViewModel
                {
                    BillId = bill.Id,
                    TradingCode = bill.TradingCode,
                    DbAmount = bill.TotalMoney ?? 0,
                    GatewayAmount = confirmationResult.Amount,
                    PaymentGatewayTransactionId = confirmationResult.PaymentGatewayTransactionId!,
                    AlertTime = DateTime.UtcNow
                };


                _backgroundJobService.Enqueue<IAlertingService>(
                alertService => alertService.SendReconciliationFailedAlertAsync(alertViewModel));

                throw new BadRequestException($"LỖI ĐỐI SOÁT: Số tiền VNPAY trả về ({confirmationResult.Amount}) không khớp với DB ({bill.TotalMoney}) cho hóa đơn {bill.Id}");
            }

            var isTransactionValidOnVnpay = await vnpayStrategy.VerifyTransactionOnGatewayAsync(confirmationResult.BillId,confirmationResult.Amount,confirmationResult.PaymentGatewayTransactionId! , confirmationResult.PayDate! , confirmationResult.TxnRef!);

            if (!isTransactionValidOnVnpay)
            {
                var alertViewModel = new SecurityAlertViewModel()
                {
                    BillId = bill.Id,
                    TradingCode = bill.TradingCode,
                    Amount = confirmationResult.Amount,
                    GatewayTransactionId = confirmationResult.PaymentGatewayTransactionId!,
                    Alertime = DateTime.UtcNow
                };
              
                _backgroundJobService.Enqueue<IAlertingService>(
                alertService => alertService.SendSecurityAlertAsync(alertViewModel));
                throw new SecurityException($"CẢNH BÁO BẢO MẬT: IPN cho hóa đơn {bill.Id} đã vượt qua các bước kiểm tra ban đầu nhưng thất bại khi xác minh lại qua API QueryDR. Có thể là một cuộc tấn công giả mạo.");
            }

            await HandleSuccessfulPaymentAsync(bill, confirmationResult.PaymentGatewayTransactionId);

            return ResponseObject<object>.ResponseSuccess("Xử lý thành công", null);
        }


        public async Task HandleSuccessfulPaymentAsync(Bill bill, string? paymentGatewayTransactionId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
              
                if (bill == null || bill.BillStatusId != (int)Domain.Enums.BillStatus.PendingPayment)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new NotFoundException($"Hóa đơn {bill!.Id} đã được xử lý hoặc không hợp lệ");
                }


                // Cập nhật lượt dùng KM, hạng thành viên, trạng thái hóa đơn...
                var promotionsToUpdate = bill.BillPromotions?.Select(bp => bp.Promotion).ToList() ?? new List<Promotion?>();
                if (promotionsToUpdate.Any())
                     _promotionService.IncrementUsageCounts(promotionsToUpdate!);

                bill.BillStatusId = (int)Domain.Enums.BillStatus.Paid;
                bill.PaymentGatewayTransactionId = paymentGatewayTransactionId;
                _billRepository.Update(bill);

                await _userService.UpdateUserRankAfterPurchaseAsync(bill.User ?? throw new NotFoundException("Không tìm thấy user"), bill.TotalMoney ?? 0);

                //Cập nhật tình trạng ghế
                var seatsToUpdate = bill.BillTickets.Select(bt => bt.Ticket?.Seat).ToList() ?? throw new NotFoundException("Không có seat");

                _seatService.UpdateSeatsAfterPurchase(seatsToUpdate!);



                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

              
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new BadRequestException($"Lỗi {ex}");
            }
        }

        public async Task<ResponseObject<object>> ProcessCashPaymentConfirmationAsync(Guid billId)
        {
            var bill = await _billRepository.GetBillDetailsForResponseAsync(billId)
                ?? throw new NotFoundException("Không tìm thấy hóa đơn để xác nhận thanh toán.");


            var cashStrategy = _strategyFactory.GetStrategy(Domain.Enums.PaymentMethod.CASH);


            var internalCallbackData = new Dictionary<string, string>
            {
               { "billId", bill.Id.ToString() },
               { "amount", bill.TotalMoney.ToString()! }
            };

            var confirmationResult = await cashStrategy.ConfirmPaymentAsync(internalCallbackData);

            if (confirmationResult.IsSuccess)
            {

                await HandleSuccessfulPaymentAsync(bill, null);

                return ResponseObject<Object>.ResponseSuccess("Thanh toán tiền mặt thành công", null);
            }

            throw new InvalidOperationException(confirmationResult.ErrorMessage);


        }
    }
}
