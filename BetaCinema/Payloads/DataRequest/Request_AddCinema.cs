﻿namespace BetaCinema.Payloads.DataRequest
{
    public class Request_AddCinema
    {
        public string Address { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string NameOfCinema { get; set; }
        public IEnumerable<Request_AddRoom>? Rooms { get; set; }
    }
}
