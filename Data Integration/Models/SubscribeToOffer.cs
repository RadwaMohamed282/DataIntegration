﻿namespace Data_Integration.Models
{
    public class SubscribeToOffer
    {
        public int Id { get; set; }
        public string CouponNumber { get; set; }
        public string MSISDN { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
    }
}
