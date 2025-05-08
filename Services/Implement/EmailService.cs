using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Interface;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MimeKit;
using System.Net.Mail;

namespace BetaCinema.Services.Implement
{
    public class EmailService : BaseService, IEmailService
    {
        private readonly EmailConfiguration _config;
        private readonly ResponseObject<DataResponseEmailMessage> _responseObject;
        private readonly ResponseObject<string> _responseTokenObject;
        private readonly EmailConverter _converter;
        private readonly UserConverter _userConverter;
        private readonly TokenService _tokenService;
        public EmailService(EmailConfiguration config,IConfiguration configuration)
        {
            _config = config;
            _responseObject = new ResponseObject<DataResponseEmailMessage>();
            _converter = new EmailConverter();
            _responseTokenObject = new ResponseObject<string>();
            _userConverter = new UserConverter();
            _tokenService = new TokenService(configuration);
        }

        public async Task<ResponseObject<string>> ConfirmEmail(string mailCode)
        {
            if (mailCode == null)
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền mailcode", null);

            var confirmCr = await _context.ConfirmEmails.FirstOrDefaultAsync(x=>x.ConfirmCode == mailCode);

            if (confirmCr == null)
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Mã code không tồn tại", null);


            if(confirmCr.ExpiredTime<DateTime.UtcNow)
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Mã đã hết hạn , vui lòng lấy mã mới", null);

            //update confirm
            confirmCr.IsConfirm = true;
            _context.ConfirmEmails.Update(confirmCr);


            var userCr = await _context.Users.FirstOrDefaultAsync(x=>x.Id == confirmCr.UserId);


            //update user
            userCr.IsActive = true;
            _context.Users.Update(userCr);
            await _context.SaveChangesAsync();

            return _responseTokenObject.ResponseSuccess("Xác nhận mail thành công", _tokenService.GenerateResetToken(userCr.Id));


        }

        public async Task<ResponseObject<DataResponseEmailMessage>> SendMail(EmailMessage emailMessage)
        {
            var message = CreateEmailMessage(emailMessage);
            await Send(message);

            var recipients = string.Join(", ", message.To);

            return _responseObject.ResponseSuccess("Gửi mail thành công",_converter.EntityToDTO(emailMessage));

        }

        
        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Email", _config.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private async Task Send (MimeMessage message)
        {
             var client = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                await client.ConnectAsync(_config.SmtpServer, _config.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(_config.UserName, _config.Password);
                await client.SendAsync(message);
            }catch (Exception ex)
            {
                throw new Exception("Lỗi khi gửi email", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
