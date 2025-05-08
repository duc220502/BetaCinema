using BetaCinema.DataContext;

namespace BetaCinema.PayLoads.Convertest
{
    public class BaseConverter
    {
        protected readonly AppDbContext _context;

        public BaseConverter()
        {
            _context = new AppDbContext();
        }
    }
}
