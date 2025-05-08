using MimeKit;

namespace BetaCinema.PayLoads.DataResponses
{
    public class DataResponseEmailMessage
    {
        public List<string> To { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }
    }
}
