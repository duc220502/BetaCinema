using BetaCinema.DataContext;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataResponses;
using System.Drawing;

namespace BetaCinema.Payloads.Convertes
{
    public class UserConverter
    {
        private readonly AppDbContext _context;

        public UserConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseUser EntityToDTO(User user)
        {
            return new DataResponseUser
            {
                Point = user.Point,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                StatusActive = user.IsActive? "Được kích hoạt": "Chưa kích hoạt",
                RankName = _context.RankCustomers.FirstOrDefault(x=>x.Id == user.RankCustomerId).Name,
                UserStatusName = _context.UserStatuses.FirstOrDefault(x=>x.Id == user.UserStatusId).Name
            };
        }
    }
}
