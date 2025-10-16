using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class CinemaService(ICinemaRepository cinemaRepository , IUnitOfWork unitOfWork , IMapper mapper) : ICinemaService
    {
        private readonly ICinemaRepository _cinemaRepository = cinemaRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        private string GenerateCinemaCode(string cinemaName)
        {

            var sanitizedName = cinemaName.Replace(" ", "").ToUpper();

            return $"CINEMA-{sanitizedName}";
        }
        public async Task<ResponseObject<DataResponseCinema>> AddCinema(Request_AddCinema rq)
        {
          
            var newCinema = new Cinema()
            {
                Name = rq.Name,
                Description = rq.Description,
                Address = rq.Address,
                Code = GenerateCinemaCode(rq.Name),
                IsActive = true,
            };

            _cinemaRepository.Add(newCinema);

            var cinemaDto = _mapper.Map<DataResponseCinema>(newCinema);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseCinema>.ResponseSuccess("Thêm Cinema thành công", cinemaDto);

        }

        public async Task<ResponseObject<DataResponseCinema>> GetCinemaById(Guid id)
        {
            var result = await _cinemaRepository.GetByIdAsync(id)?? throw new NotFoundException("Không tìm thấy Cinema");

            var dto = _mapper.Map<DataResponseCinema>(result);

            return ResponseObject<DataResponseCinema>.ResponseSuccess("Lấy thông tin cinema thành công", dto);
        }


        public async Task<ResponseObject<IEnumerable<DataResponseCinema>>> GetCinemas(Pagination pagination)
        {
            var pageResult = await _cinemaRepository.GetAllCinemasAsync(pagination);

            var results = pageResult.Data.Select(x => _mapper.Map<DataResponseCinema>(x));

            return ResponseObject<IEnumerable<DataResponseCinema>>.ResponseSuccess("Lấy danh sách Cinema thành công", results);

        }

        public async Task<ResponseObject<DataResponseCinema>> UpdateCinema(Guid id, Request_UpdateCinema rq)
        {
            var cinemaCr = await _cinemaRepository.GetCinemaByIdAsync(id)
            ?? throw new NotFoundException($"Không tìm thấy rạp chiếu phim với ID: {id}");

            if (!string.IsNullOrEmpty(rq.Name) && rq.Name != cinemaCr.Name)
            {
               
                var nameExists = await _cinemaRepository.IsNameUniqueAsync(rq.Name,id);
                if (!nameExists)
                    throw new ConflictException($"Tên rạp chiếu phim '{rq.Name}' đã tồn tại.");
            }

            _mapper.Map(rq, cinemaCr);

            _cinemaRepository.Update(cinemaCr);

            var dto = _mapper.Map<DataResponseCinema>(cinemaCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseCinema>.ResponseSuccess("Cập nhật thông tin thành công", dto);
        }

        public async Task<ResponseObject<DataResponseCinema>> DeleteCinema(Guid id)
        {
            var cinemaCr = await _cinemaRepository.GetCinemaByIdAsync(id)
              ?? throw new NotFoundException("Cinema không tồn tại.");

            cinemaCr.IsActive = false;

            _cinemaRepository.Update(cinemaCr) ;

            var dto = _mapper.Map<DataResponseCinema>(cinemaCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseCinema>.ResponseSuccess("Xóa Cinema thành công", dto);
        }
    }



}
