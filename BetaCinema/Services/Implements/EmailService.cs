using BetaCinema.Services.Interfaces;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
namespace BetaCinema.Services.Implements
{
    public class EmailService :  IEmailService
    {
        public string SendEmail(string mailTo, string subject, string body)
        {
            string appPass = "zabv yhxo wcne ksfs";
            string mailAddress = "nvduc.dhti14a12hn@sv.uneti.edu.vn";

            // Tạo đối tượng MimeMessage
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("LTS", mailAddress));
            message.To.Add(new MailboxAddress("Gủi Bạn", mailTo));
            message.Subject = subject;

            // Tạo đối tượng BodyPart để chứa nội dung email
            var bodyBuilder = new BodyBuilder();

            // Đặt nội dung email dưới dạng mã HTML
            bodyBuilder.TextBody = body;

            /* // Thêm tệp đính kèm (nếu có)
             if (attachments != null && attachments.Count > 0)
             {
                 foreach (var attachment in attachments)
                 {
                     // Đọc nội dung của file đính kèm
                     using (var memoryStream = new MemoryStream())
                     {
                         attachment.CopyTo(memoryStream);
                         memoryStream.Seek(0, SeekOrigin.Begin);

                         // Thêm file đính kèm vào BodyPart
                         bodyBuilder.Attachments.Add(attachment.FileName, memoryStream);
                     }
                 }
             }*/

            // Gán BodyPart vào MimeMessage
            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                // Gửi email
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate(mailAddress, appPass);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return "Email đã được gửi thành công.";
            }
            catch (Exception ex)
            {
                return "Lỗi khi gửi email: " + ex.Message;
            }
        }
    }
}
