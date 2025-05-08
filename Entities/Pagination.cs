namespace BetaCinema.Entities
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int ToTalCount { get; set; }
        public int ToTalPage
        {
            get
            {
                if (PageSize == 0) return 0;
                int total = ToTalCount / PageSize;
                if (ToTalCount % PageSize != 0)
                    total++;
                return total;

            }
        }

        public Pagination()
        {
            PageSize = 1;
            PageNumber = 1;
        }
    }
}
