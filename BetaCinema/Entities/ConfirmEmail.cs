﻿namespace BetaCinema.Entities
{
    public class ConfirmEmail:BaseEntity
    {
        public DateTime ExpiredTime { get; set; }
        public string ConfirmCode { get; set; }
        public bool  IsConfirm { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
