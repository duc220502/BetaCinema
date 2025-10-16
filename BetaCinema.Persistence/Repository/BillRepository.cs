using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class BillRepository(AppDbContext context) : BaseRepository<Bill>(context), IBillRepository
    {
        public async Task<List<Bill>> FindExpiredPendingBillsAsync()
        {
            var currentTime = DateTime.UtcNow;

            var expiredBills = await _context.Bills
            .Where(b => b.BillStatusId == (int)Domain.Enums.BillStatus.PendingPayment &&
                   b.ExpireAt.HasValue &&
                   b.ExpireAt < currentTime)
                .Include(b => b.BillTickets)        
                    .ThenInclude(bt => bt.Ticket)   
                    .ThenInclude(t => t!.Seat)   
            .ToListAsync();

            return expiredBills;
        }


        public async Task<Bill?> GetBillDetailsForResponseAsync(Guid billId)
        {
            var bill = await _context.Bills
            .Include(b => b.User)
            .Include(b => b.BillTickets)
                .ThenInclude(bt => bt.Ticket)
                    .ThenInclude(t => t!.Schedule)
                        .ThenInclude(s => s!.Movie) 
            .Include(b => b.BillTickets)
                .ThenInclude(bt => bt.Ticket)
                    .ThenInclude(t => t!.Schedule)
                        .ThenInclude(s => s!.Room) 
            .Include(b => b.BillTickets)
                .ThenInclude(bt => bt.Ticket)
                    .ThenInclude(t => t!.Seat)
            .Include(b => b.BillFoods)
                .ThenInclude(bf => bf.Food)
            .Include(b => b.BillPromotions)
                .ThenInclude(bp => bp.Promotion)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == billId);

            return bill;
        }

        public async Task<Bill?> GetBillForCancellationAsync(Guid billId)
        => await _context.Bills.Include(b => b.BillTickets)
            .ThenInclude(bt => bt.Ticket)
            .ThenInclude(t => t!.Seat)
            .FirstOrDefaultAsync(b => b.Id == billId);

        public async Task<Bill?> GetBillWithDetailsAsync(Guid billId)
        {
            var currentTime = DateTime.UtcNow;

            var bill = await _context.Bills
                .Include(b => b.BillStatus)
                .Include(b => b.BillTickets)
                    .ThenInclude(bt => bt.Ticket)
                        .ThenInclude(t => t!.Schedule)
                            .ThenInclude(s => s!.Movie)
                .Include(b => b.BillTickets)
                    .ThenInclude(bt => bt.Ticket)
                        .ThenInclude(t => t!.Schedule)
                            .ThenInclude(s => s!.Room)
                .Include(b => b.BillTickets)
                    .ThenInclude(bt => bt.Ticket)
                        .ThenInclude(t => t!.Seat)
                .Include(b => b.BillFoods)
                    .ThenInclude(bf => bf.Food)
                .Include(b => b.BillPromotions)
                    .ThenInclude(bp => bp.Promotion)

                .AsNoTracking()

                .FirstOrDefaultAsync(b =>
                    b.Id == billId &&
                    (b.BillStatusId == (int)Domain.Enums.BillStatus.PendingPayment || b.BillStatusId == (int)Domain.Enums.BillStatus.Paid) &&
                    (!b.ExpireAt.HasValue || b.ExpireAt > currentTime)
                );

            return bill;
        }
    }
}
