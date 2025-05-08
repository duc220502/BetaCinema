namespace BetaCinema.Entities
{
    public class GeneralSetting:BaseEntity
    {
        public DateTime BreakTime { get; set; }
        public DateTime BussinessHour { get; set; }
        public DateTime CloseTime { get; set; }
    }
}
