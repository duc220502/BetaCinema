namespace BetaCinema.Services.Interfaces
{
    public interface IEmailService
    {
         string SendEmail(string mailTo, string subject, string body/*, List<IFormFile> attachments = null*/);
    }
}
