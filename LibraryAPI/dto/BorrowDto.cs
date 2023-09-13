namespace LibraryAPI.dto
{
    public class BorrowDto
    {
        public int BorrowId { get; set; }

        public int? BookId { get; set; }

        public int? MemberId { get; set; }

        public DateTime? BorrowDate { get; set; }

        public DateTime? DueReturnDate { get; set; }

        public DateTime? ActualReturnDate { get; set; }
    }
}
