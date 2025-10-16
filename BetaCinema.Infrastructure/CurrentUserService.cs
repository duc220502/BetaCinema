using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Domain.Entities.Users;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BetaCinema.Application.Common;

namespace BetaCinema.Infrastructure
{
    public class CurrentUserService : ICurrentUserservice
    {
        /* public Guid UserId { get; }
         public string Role { get; }
         public CurrentUserService(IHttpContextAccessor httpContextAccessor)
         {
             var user = httpContextAccessor.HttpContext?.User;
             if (user is null)
             {
                 throw new UnauthorizedAccessException("Không xác thực được người dùng");
             }

             UserId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)?? throw new UnauthorizedAccessException("Không lấy được UserId"));
             Role = user.FindFirstValue(ClaimTypes.Role)?? throw new UnauthorizedAccessException("Không lấy được Role");
         }*/

        /*public Guid UserId { get; }
         * 
         * 
        public string Role { get; }*/


        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public Guid? UserId => Guid.TryParse(
                            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier),
                            out var userId) ? userId : null;

        public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;

           /* var user = httpContextAccessor.HttpContext?.User;
            if (user is null)
            {
                throw new UnauthorizedAccessException("Không xác thực được người dùng");
            }

            UserId = Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("Không lấy được UserId"));
            Role = user.FindFirstValue(ClaimTypes.Role) ?? throw new UnauthorizedAccessException("Không lấy được Role");
*/
        }

        public Guid GetRequiredUserId()
        {
            var userId = UserId;
            if (userId == null)
            {
                throw new UnauthorizedAccessException("Yêu cầu này đòi hỏi người dùng phải được xác thực.");
            }
            return userId.Value;
        }
        public string GetRequiredRoleName()
        {
            var roleName = Role;
            if (roleName == null)
            {
                throw new UnauthorizedAccessException("Yêu cầu này đòi hỏi người dùng phải được xác thực.");
            }
            return roleName;
        }

        

    }
}
