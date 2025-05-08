using BetaCinema.Entities;
using BetaCinema.PayLoads.DataResponses;
using System.Xml.Linq;

namespace BetaCinema.PayLoads.Convertest
{
    public class FoodConverter:BaseConverter
    {
        public DataResponseFood EntityToDTO(Food food)
        {

            return new DataResponseFood
            {
                Name = food.Name,
                Price = food.Price,
                Image = food.Image,
                Description = food.Description,
                StatusActive = food.IsActive?"Hoạt động":"Ngừng sản xuất"

            };
        }
    }
}
