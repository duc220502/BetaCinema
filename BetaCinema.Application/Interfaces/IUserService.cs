using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IUserService
    {
        Task<ResponseObject<DataResponseUser>> ChangePasswordAsync(Request_ChangePassword rq);

        Task<ResponseObject<DataResponseUser>> GetUserById( Guid id);

        Task<ResponseObject<IEnumerable<DataResponseUser>>> GetUsers(Pagination pagination);

        Task<ResponseObject<DataResponseUser>> GetMyProfile();

        Task<ResponseObject<DataResponseUser>> UpdateMyProfile(Request_UpdateMyProfile rq);

        Task<ResponseObject<DataResponseUser>> UpdateUserByAdmin(Guid id, Request_UpdateUserByAdmin rq);

        Task<ResponseObject<DataResponseUser>> DeleteUser(Guid id);

        Task UpdateUserRankAfterPurchaseAsync(User user, decimal amountSpent);

        Task<User> GetAndValidateCurrentUserAsync();

        Task<User> FindOrCreateExternalUserAsync(string provider,string providerKey,string email,string? name,CancellationToken ct = default);

    }
}
