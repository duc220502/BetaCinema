using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Movies;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BetaCinema.Application.UseCases
{
    public class MovieService(IMovieRepository movieRepository , IMapper mapper , IUnitOfWork unitOfWork) : IMovieService
    {
        private readonly   IMovieRepository _movieRepository = movieRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<ResponseObject<DataResponseMovie>> AddMovie(Request_AddMovie rq)
        {


            var newMovie = new Movie()
            {
                Name = rq.Name,
                Trailer = rq.Trailer,
                MovieDuration =rq.MovieDuration,
                PremiereDate = rq.PremiereDate,
                BasePrice = rq.BasePrice,
                Description  = rq.Description,
                Director = rq.Director,
                HeroImage = rq.HeroImage,
                Language = rq.Language,
                IsActive = true,
                MovieTypeId = rq.MovieTypeId,
                RateId = rq.RateId,

            };

            _movieRepository.Add(newMovie);


            var dto = _mapper.Map<DataResponseMovie>(newMovie);
            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseMovie>.ResponseSuccess("Thêm Movie thành công", dto);
        }

        public async Task<ResponseObject<DataResponseMovie>> DeleteMovie(Guid id)
        {
            var movieCr = await _movieRepository.GetByIdAsync(id)
              ?? throw new NotFoundException("Movie không tồn tại.");

            movieCr.IsActive = false;

            _movieRepository.Update(movieCr);

            var dto = _mapper.Map<DataResponseMovie>(movieCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseMovie>.ResponseSuccess("Xóa Movie thành công", dto);
        }

        public async Task<ResponseObject<DataResponseMovie>> GetMovieById(Guid id)
        {
            var result = await _movieRepository.GetByIdAsync(id) ?? throw new NotFoundException("Không tìm thấy Movie");

            var dto = _mapper.Map<DataResponseMovie>(result);

            return ResponseObject<DataResponseMovie>.ResponseSuccess("Lấy thông tin Movie thành công", dto);
        }

        public async Task<ResponseObject<DataResponseMovie>> UpdateMovie(Guid id, Request_UpdateMovie rq)
        {
            var movieCr = await _movieRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Không tìm thấy movie với ID: {id}");

            if (!string.IsNullOrEmpty(rq.Name) && rq.Name != movieCr.Name)
            {

                var nameExists = await _movieRepository.IsNameUniqueAsync(rq.Name, id);
                if (!nameExists)
                    throw new ConflictException($"Tên movie '{rq.Name}' đã tồn tại.");
            }

            _mapper.Map(rq, movieCr);

            _movieRepository.Update(movieCr);

            var dto = _mapper.Map<DataResponseMovie>(movieCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseMovie>.ResponseSuccess("Cập nhật thông tin thành công", dto);
        }
    }
}
