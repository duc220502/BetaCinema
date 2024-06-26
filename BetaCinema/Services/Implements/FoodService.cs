﻿using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.Payloads.Convertes;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BetaCinema.Services.Implements
{
    public class FoodService : BaseService, IFoodService
    {
        private readonly ResponseObject<DataResponseFood> _responseObject;
        private readonly FoodConverter _converter;

        public FoodService()
        {
            _converter = new FoodConverter();
            _responseObject = new ResponseObject<DataResponseFood>();
        }
        public ResponseObject<DataResponseFood> CreateFood(Request_AddFood rq)
        {
            if (InputHelper.checkNull(rq.Price.ToString(), rq.Description, rq.Img   , rq.NameOfFood))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đủ thông tin", null);

            if (rq.Price <= 0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Gía không được nhỏ hơn = 0", null);

            Food food = new Food()
            {
                Price = rq.Price,
                Description = rq.Description,
                Img = rq.Img,
                NameOfFood = rq.NameOfFood,
                IsActive = true

            };

            _context.Foods.Add(food);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Thêm thành công", _converter.EntityToDTO(food));

        }

        public ResponseObject<DataResponseFood> DeleteFood(int id)
        {
            if (InputHelper.checkNull(id.ToString()))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập id", null);

            var foodCr = _context.Foods.FirstOrDefault(x => x.Id == id);
            if (foodCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Id không tồn tại", null);

            foodCr.IsActive = false;
            _context.Foods.Update(foodCr);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Xóa thành công", _converter.EntityToDTO(foodCr));
        }

        public ResponseObject<DataResponseBestSellingFood> Get_BestSaller()
        {
            ResponseObject < DataResponseBestSellingFood > response= new ResponseObject<DataResponseBestSellingFood>();
            DateTime startDate = DateTime.Now.AddDays(-7);
            DateTime endDate = DateTime.Now;

         
            var bestSellingFood = _context.BillFoods
                .Include(bf => bf.Food)
                .Where(bf => bf.Bill.CreateTime >= startDate && bf.Bill.CreateTime <= endDate)
                .GroupBy(bf => bf.Food)
                .Select(group => new
                {
                    Food = group.Key.NameOfFood,
                    Quantity = group.Sum(bf => bf.Quantity)
                })
                .OrderByDescending(result => result.Quantity)
                .FirstOrDefault();


            if (bestSellingFood == null)
                return response.ResponseError(StatusCodes.Status400BadRequest, "Không có mặt hàng nào bestSaller", null);

            var dataResponse = new DataResponseBestSellingFood
            {
                FoodName = bestSellingFood.Food,
                QuantitySold = bestSellingFood.Quantity
            };

            return response.ResponseSuccess("Thành công", dataResponse);

        }

        public ResponseObject<DataResponseFood> UpdateFood(int id, Request_UpdateFood rq)
        {
            if (InputHelper.checkNull(rq.Price.ToString(), rq.Description, rq.Img, rq.NameOfFood))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng nhập đủ thông tin", null);

            if (rq.Price <= 0)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Gía không được nhỏ hơn = 0", null);

            var foodCr = _context.Foods.FirstOrDefault(x=>x.Id == id);

            foodCr.Price = rq.Price;
            foodCr.Description = rq.Description;
            foodCr.Img = rq.Img;
            foodCr.NameOfFood = rq.NameOfFood;

            _context.Foods.Update(foodCr);
            _context.SaveChanges();


            return _responseObject.ResponseSuccess("Thành công",_converter.EntityToDTO(foodCr));
            
        }
    }
}
