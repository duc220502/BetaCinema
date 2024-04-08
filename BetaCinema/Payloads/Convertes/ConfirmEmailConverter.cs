using BetaCinema.DataContext;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataResponses;

namespace BetaCinema.Payloads.Convertes
{
    public class ConfirmEmailConverter
    {
        private readonly AppDbContext _context;

        public ConfirmEmailConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseConfirmEmail EntityToDTO(ConfirmEmail cf)
        {
            return new DataResponseConfirmEmail
            {
                ExpiredTime = cf.ExpiredTime,
                ConfirmCode = cf.ConfirmCode,
                StatusConfirm = cf.IsConfirm ? "Đã xác nhận" : "Chưa xác nhận",
                UserName = _context.Users.FirstOrDefault(x => x.Id == cf.UserId).Name
            };
        }
    }
}
