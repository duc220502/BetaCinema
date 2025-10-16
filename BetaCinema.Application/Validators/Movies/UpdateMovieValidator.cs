using BetaCinema.Application.DTOs.DataRequest.Movies;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Movies
{
    public class UpdateMovieValidator : AbstractValidator<Request_UpdateMovie>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IRateRepository _ratingRepository;
        private readonly IMovieTypeRepository _movieTypeRepository;
        public UpdateMovieValidator(IMovieRepository movieRepository, IRateRepository ratingRepository, IMovieTypeRepository movieTypeRepository) 
        { 
            _movieRepository = movieRepository;
            _ratingRepository = ratingRepository;
            _movieTypeRepository = movieTypeRepository;

            RuleFor(x => x.MovieDuration)
           .NotNull().NotEmpty().WithMessage("Thời lượng phim không được để trống")
           .GreaterThanOrEqualTo(0).WithMessage("Thời lượng phim không hợp lí.")
           .When(x => x.MovieDuration.HasValue);

            RuleFor(x => x.BasePrice)
          .NotNull().NotEmpty().WithMessage("Gía cơ bản phim không được để trống")
          .GreaterThanOrEqualTo(0).WithMessage("Gía phim không hợp lí.")
          .When(x => x.BasePrice.HasValue);

            RuleFor(x => x.PremiereDate)
           .NotNull().NotEmpty().WithMessage("Thời gian phát hành không được để trống")
           .GreaterThan(DateTime.UtcNow).WithMessage("Thời gian phát hành phải lớn hơn ngày hiện tại.")
           .When(x=>x.PremiereDate.HasValue);

            RuleFor(x => x.MovieTypeId)
            .NotEqual(Guid.Empty).WithMessage("ID là bắt buộc và không được để trống.")
            .MustAsync(CheckMovieType).WithMessage("MovieType không tồn tại")
            .When(x=>x.MovieTypeId.HasValue);

            RuleFor(x => x.RateId)
            .Must(id => Enum.IsDefined(typeof(Rating), id!))
            .WithMessage("Giá trị RateId không hợp lệ hoặc không tồn tại.")
            .When(x=>x.RateId.HasValue);
        }

        private async Task<bool> CheckMovieType(Guid? id, CancellationToken cancellationToken)
          => await _movieTypeRepository.GetByIdAsync(id) != null;
    }
}
