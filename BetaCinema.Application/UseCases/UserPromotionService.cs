using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.UserPromotions;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class UserPromotionService(IUserPromotionRepository userPromotionRepository,IUnitOfWork unitOfWork,IMapper mapper) : IUserPromotionService
    {

        private readonly IUserPromotionRepository _userPromotionRepository = userPromotionRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ResponseObject<DataResponseUserPromotionPersonal>> AddUserPromotionAsync(Request_AddUserPromotion rq)
        {
            var newUserPromotion = new UserPromotion()
            {
                Quantity = rq.Quantity??1,
                IsActive = true,
                PromotionId = rq.PromotionId,
                UserId = rq.UserId,
            };

            _userPromotionRepository.Add(newUserPromotion);


            var dto = _mapper.Map<DataResponseUserPromotionPersonal>(newUserPromotion);
            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseUserPromotionPersonal>.ResponseSuccess("Thêm UserPromotion thành công", dto);
        }

        public async Task<ResponseObject<DataResponseUserPromotionPersonal>> DeleteUserPromotion(Guid id)
        {
            var userPromotionCr = await _userPromotionRepository.GetByIdWithDetailsAsync(id)
             ?? throw new NotFoundException("UserPromotion không tồn tại.");

            userPromotionCr.IsActive = false;

            _userPromotionRepository.Update(userPromotionCr);

            var dto = _mapper.Map<DataResponseUserPromotionPersonal>(userPromotionCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseUserPromotionPersonal>.ResponseSuccess("Xóa UserPromotion thành công", dto);
        }

        public async Task<ResponseObject<DataResponseUserPromotionPersonal>> GetUserPromotionByIdAsync(Guid id)
        {
            var result = await _userPromotionRepository.GetByIdWithDetailsAsync(id) ?? throw new NotFoundException("Không tìm thấy UserPromotion");

            var dto = _mapper.Map<DataResponseUserPromotionPersonal>(result);

            return ResponseObject<DataResponseUserPromotionPersonal>.ResponseSuccess("Lấy thông tin UserPromotion thành công ", dto);
        }

        public async Task<ResponseObject<DataResponseUserPromotionPersonal>> UpdateUserPromotion(Guid id, Request_UpdateUserPromotion rq)
        {
            var userPromotionCr = await _userPromotionRepository.GetByIdWithDetailsAsync(id)
            ?? throw new NotFoundException($"Không tìm thấy UserPromotion với ID: {id}");

            _mapper.Map(rq, userPromotionCr);

            _userPromotionRepository.Update(userPromotionCr);

            var dto = _mapper.Map<DataResponseUserPromotionPersonal>(userPromotionCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseUserPromotionPersonal>.ResponseSuccess("Cập nhật thông tin thành công", dto);
        }


    }
}
