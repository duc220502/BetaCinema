using BetaCinema.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.AI
{
    public interface IChatMovieQueryService
    {
        Task<IReadOnlyList<MovieChatDto>> GetNowShowingAsync(DateOnly date, CancellationToken ct);

        Task<IReadOnlyList<ShowtimeChatDto>> GetShowtimesByMovieAfterAsync(string movieName,DateOnly date, TimeOnly afterTime,CancellationToken ct);
    }
}
