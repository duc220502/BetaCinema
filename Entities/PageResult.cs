namespace BetaCinema.Entities
{
    public class PageResult<T>
    {
        public Pagination Pagination { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PageResult(Pagination pagination, IEnumerable<T> data)
        {
            Pagination = pagination;
            Data = data;
        }

        public static  IQueryable<T> ToPageResult(Pagination pagination, IQueryable<T> Data)
        {
            pagination.PageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;

            Data =  Data.Skip(pagination.PageSize * (pagination.PageNumber - 1)).Take(pagination.PageSize).AsQueryable();
            return Data;
        }
    }
}
