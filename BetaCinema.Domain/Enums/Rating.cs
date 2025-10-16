using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Enums
{
    public enum Rating
    {
        P = 1,  // Mọi lứa tuổi
        C13 = 2, // Cấm trẻ em dưới 13 tuổi
        C16 = 3, // Cấm trẻ em dưới 16 tuổi
        C18 = 4  // Cấm trẻ em dưới 18 tuổi
    }
}
