using BetaCinema.Entities;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace BetaCinema.PayLoads.Convertest
{
    public class UserConverter : BaseConverter
    {

       
        public DataResponseUser EntityToDTO(User user)
        {
            var roleCr = _context.Roles.FirstOrDefault(x=>x.Id == user.RoleId);
            var userStatusCr = _context.UserStatuses.FirstOrDefault(x=> x.Id == user.UserStatusId);   
            return new DataResponseUser
            {
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                NumberPhone = user.NumberPhone,
                Point = user.Point,
                Active = user.IsActive?"Kích Hoạt" : "Chưa kích hoạt",
                RoleName = roleCr?.RoleName??"Không có rulename",
                Status = userStatusCr?.Name ?? "Không có status"
            };
        }
    }
}
