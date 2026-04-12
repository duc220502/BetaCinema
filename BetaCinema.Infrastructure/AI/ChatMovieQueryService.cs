using BetaCinema.Application.DTOs.AI;
using BetaCinema.Application.Interfaces.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.AI
{
    public class ChatMovieQueryService : IChatMovieQueryService
    {
        public Task<IReadOnlyList<MovieChatDto>> GetNowShowingAsync(DateOnly date, CancellationToken ct)
        {
            IReadOnlyList<MovieChatDto> result =
        [
            new(){ MovieId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),Title =  "Dune 2",Genre =  "Khoa học viễn tưởng",DurationMinutes=  166 },
             new(){ MovieId  = Guid.Parse("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),Title =  "Congfu panda 2",Genre =  "Hành động",DurationMinutes=  123 },

        ];

            return Task.FromResult(result);
        }

        public Task<IReadOnlyList<ShowtimeChatDto>> GetShowtimesByMovieAfterAsync(string movieName, DateOnly date, TimeOnly afterTime, CancellationToken ct)
        {
            var day = date.ToDateTime(TimeOnly.MinValue);

            IReadOnlyList<ShowtimeChatDto> result =
            [
                new(){ ShowtimeId = Guid.NewGuid(), MovieTitle =  movieName, StartTime = day.AddHours(20).AddMinutes(15),RoomName =  "Phòng 1" },
                 new(){ ShowtimeId = Guid.NewGuid(),MovieTitle =  movieName,StartTime =  day.AddHours(21).AddMinutes(0),RoomName =  "Phòng 3" }
            ];

            return Task.FromResult(result);
        }
    }
}
