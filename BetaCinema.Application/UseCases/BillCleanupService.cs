using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Interfaces.Repositorys;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class BillCleanupService(IUnitOfWork unitOfWork , IBillRepository billRepository , ISeatRepository seatRepository , ILogger<BillCleanupService> logger) : IBillCleanUpService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork; 
        private readonly IBillRepository _billRepository = billRepository;
        private readonly ISeatRepository _seatRepository = seatRepository;
        private readonly ILogger<BillCleanupService> _logger = logger;
        public  async Task ProcessExpiredBillsAsync()
        {
            var expiredBills = await _billRepository.FindExpiredPendingBillsAsync();

            if (!expiredBills.Any())
                return;

            foreach (var bill in expiredBills)
            {
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    
                    bill.BillStatusId = (int)Domain.Enums.BillStatus.Expired;

                    var ticketsToInvalidate = bill.BillTickets.Select(bt => bt.Ticket).ToList() ?? throw new NotFoundException("Không tìm thấy ticket");
                    ticketsToInvalidate.ForEach(t => t!.IsActive = false);

                    var seatsToRelease = ticketsToInvalidate.Select(t => t!.Seat).ToList();
                    seatsToRelease.ForEach(s => s!.SeatStatusId = (int)Domain.Enums.SeatStatus.Available);

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw new Exception($"Lỗi {ex}");
                }
            }
        }

        public async Task ReleaseExpiredSeatsForScheduleAsync(Guid scheduleId)
        {
            int releasedSeatsCount = await _seatRepository.ReleaseExpiredHeldSeatsForScheduleAsync(scheduleId);

            if (releasedSeatsCount > 0)
            {
                _logger.LogInformation("Lazy cleanup: Đã giải phóng {count} ghế cho suất chiếu {scheduleId}", releasedSeatsCount, scheduleId);
            }
        }
    }
}
