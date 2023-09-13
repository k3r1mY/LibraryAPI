namespace LibraryAPI.dto
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }

        public int? BookId { get; set; }

        public int? MemberId { get; set; }

        public DateTime? ReservationDate { get; set; }

        public DateTime? PickupDate { get; set; }
    }
}
