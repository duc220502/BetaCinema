using BetaCinema.Application.DTOs;
using BetaCinema.Application.DTOs.ViewModelEmail;
using BetaCinema.Application.Enums;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class ConfirmationEmailService (IRazorTemplateService razorService , IEmailService emailService) : IConfirmationEmailService
    {
        private readonly IRazorTemplateService _razorService = razorService;
        private readonly IEmailService _emailService = emailService;

        public async Task SendConfirmationEmailAsync(ConfirmationEmailRequest request)
        {
            var (templateName, subject, model) = BuildTemplateInfo(request);

            var body = await _razorService.RenderTemplateAsync(templateName, model);

            await _emailService.SendEmailAsync(request.UserEmail, subject, body);
        }

        private (string templateName, string subject, object model) BuildTemplateInfo(
        ConfirmationEmailRequest request)
        {
            return request.Purpose switch
            {
                CodePurpose.Register
                    => BuildRegisterTemplate(request),

                CodePurpose.PasswordReset
                    => BuildResetPasswordTemplate(request),

                _ => throw new ArgumentOutOfRangeException(nameof(request.Purpose), request.Purpose, null)
            };
        }
        private (string templateName, string subject, object model) BuildRegisterTemplate(
        ConfirmationEmailRequest request)
        {
            if (request.Method == ConfirmationMethod.LINK)
            {
                var link = $"{request.CallbackUrl}?token={request.Token}";
                return ("RegisterLink.cshtml",
                    "Xác nhận đăng ký tài khoản",
                    new RegisterLinkEmailModel
                    {
                        UserEmail = request.UserEmail,
                        VerifyLink = link
                    });
            }

            return ("RegisterOtpEmailModel.cshtml",
                "Mã OTP xác nhận đăng ký tài khoản",
                new RegisterOtpEmailModel
                {
                    UserEmail = request.UserEmail,
                    Otp = request.Token
                });
        }

        private (string templateName, string subject, object model) BuildResetPasswordTemplate(
         ConfirmationEmailRequest request)
        {
            if (request.Method == ConfirmationMethod.LINK)
            {
                var link = $"{request.CallbackUrl}?token={request.Token}";
                return ("ResetPasswordLink.cshtml",
                    "Đặt lại mật khẩu tài khoản của bạn",
                    new ResetPasswordLinkEmailModel
                    {
                        UserEmail = request.UserEmail,
                        ResetLink = link
                    });
            }

            return ("ResetPasswordOtp.cshtml",
                "Mã OTP đặt lại mật khẩu",
                new ResetPasswordOtpEmailModel
                {
                    UserEmail = request.UserEmail,
                    Otp = request.Token
                });
        }

    }
}
