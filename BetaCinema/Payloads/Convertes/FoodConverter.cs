using BetaCinema.DataContext;
using BetaCinema.Entities;
using BetaCinema.Payloads.DataResponses;

namespace BetaCinema.Payloads.Convertes
{
    public class FoodConverter
    {
        private readonly AppDbContext _context;

        public FoodConverter()
        {
            _context = new AppDbContext();
        }
        public DataResponseFood EntityToDTO(Food food)
        {
            return new DataResponseFood
            {
                Price = food.Price,
                Description = food.Description,
                Img = food.Img,
                NameOfFood = food.NameOfFood,
                ActiveStatus = food.IsActive?"Hoạt động":"Không hoạt động"
            };
        }
    }
}
