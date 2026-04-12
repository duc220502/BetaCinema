using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.Auth.Requests;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.Interfaces.Auths;
using BetaCinema.Application.Interfaces.Catching;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases.Auths
{
    public class ExternalLinkingService(IExternalLinkingStore  externalLinkingStore , IExternalLoginRepository externalLoginRepository ,
        IUserRepository  userRepository , IUnitOfWork unitOfWork , ITokenService tokenService , IRateLimiter rateLimiter , IOtpService otpService,
        IEmailService emailService) : IExternalLinkingService
    {
        private readonly IExternalLinkingStore _externalLinkingStore = externalLinkingStore;
        private readonly IExternalLoginRepository _externalLoginRepository = externalLoginRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IRateLimiter _rateLimiter = rateLimiter;
        private readonly IOtpService _otpService = otpService;
        private readonly IEmailService _emailService = emailService;

        public async Task<ResponseObject<DataResponseToken>> ConfirmLinkAsync(ConfirmExternalLinkRequest req, CancellationToken ct)
        {

            Console.WriteLine($"LinkingToken: {req.LinkingToken} ne 11111");
            Console.WriteLine($"Otp: {req.Otp}");


            var state = await _externalLinkingStore.GetStateAsync(req.LinkingToken, ct);
            if (state == null)
                throw new BadRequestAppException("Phiên liên kết đã hết hạn. Vui lòng đăng nhập Google lại.");


            if (await _externalLinkingStore.IsUsedAsync(req.LinkingToken, ct))
                throw new BadRequestAppException("Phiên liên kết đã được sử dụng.");

            var ok = await _externalLinkingStore.VerifyOtpAsync(req.LinkingToken, req.Otp.Trim(), ct);
            if (!ok)
            {
                var fail = await _externalLinkingStore.IncrementFailCountAsync(req.LinkingToken, ct);
                if (fail >= 5)
                    throw new BadRequestAppException("OTP sai quá nhiều lần. Vui lòng thử lại sau.");

                throw new BadRequestAppException("OTP không đúng hoặc đã hết hạn.");
            }

            var existing = await _externalLoginRepository.GetExistingLink(state.Provider, state.ProviderKey, ct);
            if (existing != null)
                throw new BadRequestAppException("Tài khoản bên thứ 3 đã được liên kết.");


            _externalLoginRepository.Add(new ExternalLogin
            {
                Provider = state.Provider,
                ProviderKey = state.ProviderKey,
                UserId = state.UserId
            });

            await _unitOfWork.SaveChangesAsync(ct);
            await _externalLinkingStore.MarkUsedAsync(req.LinkingToken, ct);


            var user = await _userRepository.GetByIdWithRoleAsync(state.UserId, ct)
                   ?? throw new NotFoundAppException("Không có user");

            var token = await _tokenService.GenerateTokenAsync(user);

            return ResponseObject<DataResponseToken>.ResponseSuccess("Xác thực liên kết đăng nhập thành công", token);

        }

        public async Task<ResponseObject<object>> ResendOtpAsync(ResendExternalLinkOtpRequest req, CancellationToken ct)
        {
            var state = await _externalLinkingStore.GetStateAsync(req.LinkingToken, ct);
            if (state == null)
                throw new BadRequestAppException("Phiên liên kết đã hết hạn. Vui lòng đăng nhập Google lại.");

            if (await _externalLinkingStore.IsUsedAsync(req.LinkingToken, ct))
                throw new BadRequestAppException("Phiên liên kết đã được sử dụng.");

            // chống spam resend: 1 lần / 60 giây theo email
            var allowed = await _rateLimiter.HitAsync(
                route: "external-link-resend-otp",
                subject: state.Email,
                limit: 1,
                window: TimeSpan.FromMinutes(1));

            if (!allowed)
                throw new BadRequestAppException("Bạn thao tác quá nhanh. Vui lòng thử lại sau 1 phút.");

            var otp = _otpService.GenerateNumericCode(6);

            await _externalLinkingStore.SaveOtpAsync(
                req.LinkingToken,
                otp,
                ttl: TimeSpan.FromMinutes(5),
                ct);

           
            var subject = "OTP xác thực liên kết đăng nhập";
            var body = $"Mã xác thực của bạn là : {otp}";
            await _emailService.SendEmailAsync(state.Email, subject, body);

            return ResponseObject<object>.ResponseSuccess("Gửi lại mã otp thành công ", null);
        }
    }


}
