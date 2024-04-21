namespace BetaCinema.Payloads.DataResponses
{
    public class DataResponseMovieDetail
    {
        public string MovieName { get; set; }
        public IEnumerable<DataResponseMovieRoom> Rooms { get; set; }
    }
}
