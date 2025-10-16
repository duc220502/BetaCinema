using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataRequest.Foods;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class FoodService(IFoodRepository foodRepository , IMapper mapper , IUnitOfWork unitOfWork) : IFoodService
    {
        private readonly IFoodRepository _foodRepository = foodRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ResponseObject<DataResponseFood>> AddFood(Request_AddFood rq)
        {
            var newFood = new Food()
            {
                Name = rq.Name,
                Price = rq.Price?? 0 ,
                Image = rq.Image,
                Description = rq.Description,
                IsActive = true
            };
            _foodRepository.Add(newFood);

            var dto = _mapper.Map<DataResponseFood>(newFood);
           await _unitOfWork.SaveChangesAsync();

           return ResponseObject<DataResponseFood>.ResponseSuccess("Thêm Food thành công", dto);
        }

        public async Task<ResponseObject<DataResponseFood>> DeleteFood(Guid id)
        {
            var fooCr = await _foodRepository.GetFoodByIdAsync(id)
             ?? throw new NotFoundException("Food không tồn tại.");

            fooCr.IsActive = false;

            _foodRepository.Update(fooCr);

            var dto = _mapper.Map<DataResponseFood>(fooCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseFood>.ResponseSuccess("Xóa Food thành công", dto);
        }

        public async Task<ResponseObject<DataResponseFood>> GetFoodById(Guid id)
        {
            var result = await _foodRepository.GetByIdAsync(id)??throw new NotFoundException("Không tìm thấy Food");

            var dto = _mapper.Map<DataResponseFood>(result);

            return ResponseObject<DataResponseFood>.ResponseSuccess("Lấy thông tin Food thành công", dto);
        }

        public async Task<List<PreparedFoodDto>> PrepareFoodItemsAsync(List<Request_FoodItem>? requestItems)
        {
            if (requestItems == null || !requestItems.Any())
                return new List<PreparedFoodDto>();

            var aggregatedRequestItems = requestItems
            .GroupBy(item => item.FoodId) 
            .Select(group => new Request_FoodItem() { FoodId = group.Key , Quantity = group.Sum(item => item.Quantity) }).ToList();

            var distinctFoodIds = aggregatedRequestItems.Select(x => x.FoodId).Distinct().ToList();

            var foodsFromDb = await _foodRepository.GetFoodsByIdsAsync(distinctFoodIds);

            if (foodsFromDb?.Count != distinctFoodIds.Count)
                throw new NotFoundException($"Một hoặc nhiều món ăn không tồn tại.");

            var preparedFoods = new List<PreparedFoodDto>();

            foreach (var itemRequest in aggregatedRequestItems)
            {
                var foodEntity = foodsFromDb.First(f => f.Id == itemRequest.FoodId);

                if (itemRequest.Quantity <= 0)
                {
                    throw new InvalidOperationException($"Số lượng cho món '{foodEntity.Name}' phải lớn hơn 0.");
                }


                preparedFoods.Add(new PreparedFoodDto() { FoodId = foodEntity.Id, Quantity = itemRequest.Quantity, Price = foodEntity.Price, FoodName = foodEntity.Name });

            }
            return preparedFoods;
        }

        public async Task<ResponseObject<DataResponseFood>> UpdateFood(Guid id,Request_UpdateFood rq)
        {
            var foodCr = await _foodRepository.GetFoodByIdAsync(id)
            ?? throw new NotFoundException($"Không tìm thấy food với ID: {id}");

            if (!string.IsNullOrEmpty(rq.Name) && rq.Name != foodCr.Name)
            {

                var nameExists = await _foodRepository.IsFoodNameUniqueAsync(rq.Name, id);
                if (!nameExists)
                    throw new ConflictException($"Tên food '{rq.Name}' đã tồn tại.");
            }

            _mapper.Map(rq, foodCr);

            _foodRepository.Update(foodCr);

            var dto = _mapper.Map<DataResponseFood>(foodCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseFood>.ResponseSuccess("Cập nhật thông tin thành công", dto);
        }
    }
}
