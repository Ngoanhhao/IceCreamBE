﻿namespace IceCreamBE.DTO
{
    public class FeedbackDTO
    {
        public int Id { get; set; }
        public string feedBack_product { get; set; }
        public int accountID { get; set; }
        public DateTime release_date { get; set; }
    }
}
