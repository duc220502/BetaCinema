using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Interface;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BetaCinema.Services.Implement
{
    public class FoodService : BaseService, IFoodService
    {
        private readonly ResponseObject<DataResponseFood> _responseObject;

        private readonly FoodConverter _foodConverter;

        public FoodService()
        {
            _responseObject = new ResponseObject<DataResponseFood>();
            _foodConverter = new FoodConverter();   
        }

        public async Task<ResponseObject<DataResponseFood>> AddFood(Request_AddFood rq)
        {
            if (rq == null || InputHelper.checkNull(new string[] { rq.Name, rq.Price.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền thông tin", null);

            var checkName = await _context.Foods.AnyAsync(x=>x.Name == rq.Name && x.IsActive == true);
            if (checkName)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên food bị trùng", null);


            if(rq.Price<0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Gía không hợp lệ", null);


            var newFood = new Food()
            {
                Name = rq.Name,
                Price = rq.Price,
                Image = rq.Image,
                Description = rq.Description,
                IsActive = true

            };

            _context.Foods.Add(newFood);
            await _context.SaveChangesAsync();

                return _responseObject.ResponseSuccess( "Thêp food thành công", _foodConverter.EntityToDTO(newFood));
        }

        public async Task<ResponseObject<DataResponseFood>> DeleteFood(int id)
        {
            if (InputHelper.checkNull(new string[] { id.ToString() }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id cần xóa", null);

            var foodCr = await _context.Foods.FirstOrDefaultAsync(x=>x.Id == id && x.IsActive == true);

            if(foodCr == null)

                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Food không tồn tại hoặc không hoạt động", null);

            foodCr.IsActive = false;

            _context.Foods.Update(foodCr);

             await _context.SaveChangesAsync();

            return _responseObject.ResponseSuccess("Xóa food thành công", _foodConverter.EntityToDTO(foodCr));
        }

        public async Task<ResponseObject<DataResponseFood>> UpdateFood(Request_UpdateFood rq)
        {
            if(rq == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền thông tin", null);

            var foodCr = await _context.Foods.FirstOrDefaultAsync(x=>x.Id == rq.Id && x.IsActive == true);

            if(foodCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Food không tồn tại", null);

            var isDuplicateName = await _context.Foods.AnyAsync(x => x.Id != rq.Id && x.Name == rq.Name && x.IsActive == true);

            if(rq.Price < 0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Gía không hợp lệ", null);

            if (isDuplicateName)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Name Food bị trùng", null);


            foodCr.Name = rq.Name??foodCr.Name;
            foodCr.Price = rq.Price??foodCr.Price;
            foodCr.Image = rq.Image ?? foodCr.Image;
            foodCr.Description = rq.Description ?? foodCr.Description;  


            _context.Foods.Update(foodCr);
            await _context.SaveChangesAsync();
            return _responseObject.ResponseSuccess("Cập nhật thành công", _foodConverter.EntityToDTO(foodCr));


        }
    }
}
