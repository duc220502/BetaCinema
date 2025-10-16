using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BetaCinema.Application.UseCases
{
    public class BookingService(ICurrentUserservice currentUserservice , IFoodService foodService,IMapper mapper ,
        ITicketService ticketService,IScheduleRepository scheduleRepository , ISeatRepository seatRepository , 
        IBillService billService,IBillRepository billRepository,IUnitOfWork unitOfWork , IUserService userService): IBookingService
    {

        private readonly ICurrentUserservice _currentUserservice = currentUserservice;
        private readonly IUserService _userService = userService;
        private readonly ITicketService _ticketService = ticketService;
        private readonly IFoodService _foodService = foodService; 
        private readonly IScheduleRepository _scheduleRepository = scheduleRepository;
        private readonly ISeatRepository _seatRepository = seatRepository;
        private readonly IBillService _billService = billService;
        private readonly IBillRepository _billRepository = billRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task CancelPendingBookingAsync(Guid billId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var bill = await _billRepository.GetBillForCancellationAsync(billId)
                ?? throw new NotFoundException("Không tìm thấy hóa đơn.");

                var currentUser = await _userService.GetAndValidateCurrentUserAsync();

                bool isOwner = bill.UserId == currentUser.Id;
                bool isAdmin = currentUser.RoleId == (int)Domain.Enums.UserRole.Admin;

                if (!isOwner &&  !isAdmin)
                {
                    throw new ForbiddenException("Bạn không có quyền thực hiện hành động này.");
                }

                if (bill.BillStatusId != (int)Domain.Enums.BillStatus.PendingPayment && bill.BillStatusId != (int)Domain.Enums.BillStatus.Failed)
                {
                    throw new InvalidOperationException("Chỉ có thể hủy hóa đơn đang chưa  thanh toán.");
                }

                bill.BillStatusId = (int)Domain.Enums.BillStatus.CancelledByUser;

                var tickets = bill.BillTickets.Select(bt => bt.Ticket).ToList();
                if (tickets.Any())
                {
                    
                    tickets.ForEach(t => t!.IsActive = false);

                    var seatsToRelease = tickets.Select(t => t!.Seat).ToList();
                    seatsToRelease.ForEach(s => {
                        if (s != null)
                            s.SeatStatusId = (int)Domain.Enums.SeatStatus.Available;
                    });
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new BadRequestException($"Lỗi {ex}");
            }
        }

        public async Task<ResponseObject<DataResponseBill>> CreateBookingAsync(Request_CreateBooking request)
        {
            var userId = (await _userService.GetAndValidateCurrentUserAsync()).Id;

            var schedule = await _scheduleRepository.GetScheduleDetailByIdAsync(request.ScheduleId)
                ?? throw new NotFoundException("Không tìm thấy schedule");

            var preparationTicketResult = await _ticketService.PrepareTicketsForBookingAsync(schedule,request.SeatIds);
            var foodDtos = await _foodService.PrepareFoodItemsAsync(request.FoodItems);           

            var finalBill = await _billService.CreateBillFromItemsAsync(userId, foodDtos, preparationTicketResult.TicketDtos, request.PromotionIds);

            _billRepository.Add(finalBill);

            preparationTicketResult.ValidatedSeats.ForEach(seat => {
                seat.SeatStatusId = (int)Domain.Enums.SeatStatus.Held;
            });

          /*  _seatRepository.UpdateRange(preparationTicketResult.ValidatedSeats);*/

            await _unitOfWork.SaveChangesAsync();

            var fullBillEntity = await _billRepository.GetBillDetailsForResponseAsync(finalBill.Id);
            if (fullBillEntity == null)
                throw new InvalidOperationException("Không thể tải lại chi tiết hóa đơn vừa tạo.");

            var dto = _mapper.Map<DataResponseBill>(fullBillEntity);
            return ResponseObject<DataResponseBill>.ResponseSuccess("Tạo hóa đơn thành công",dto);

        }
    }
}
