namespace Data_Integration.Models
{
    public class SubscribeToOfferFTP
    {
        public int Id { get; set; }
        public string OfferNumbers { get; set; }
        public string MSISDN { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
    }
}
