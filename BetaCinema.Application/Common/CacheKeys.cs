using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Common
{
    public static class CacheKeys
    {
        public static string Movie(Guid id) => $"movie:{id}";
        public static string NowShowing(Guid cinemaId) => $"cinema:{cinemaId}:now-showing";
        public static string ShowTimes(Guid cinemaId, DateOnly d) => $"showtimes:{cinemaId}:{d:yyyyMMdd}";
    }

}
