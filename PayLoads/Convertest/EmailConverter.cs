using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.DataResponses;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Cms;

namespace BetaCinema.PayLoads.Convertest
{
    public class EmailConverter
    {
        public DataResponseEmailMessage EntityToDTO(EmailMessage emailMessage)
        {

            return new DataResponseEmailMessage
            {
                To = emailMessage.To.Select(r => r.Address).ToList(),
                Subject = emailMessage.Subject,
                Content = emailMessage.Content  

            };
        }
    }
}
