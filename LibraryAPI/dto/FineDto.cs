namespace LibraryAPI.dto
{
    public class FineDto
    {
        public int FineId { get; set; }

        public int? MemberId { get; set; }

        public decimal? Amount { get; set; }

        public string? PaymentStatus { get; set; }

        public DateTime? DatePaid { get; set; }
    }
}
