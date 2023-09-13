namespace LibraryAPI.dto
{
    public class BookDto
    {
        public int BookId { get; set; }
        public string? Isbn { get; set; }
        public string? Title { get; set; }

        // Include Author Name
        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }

        // Include Genre Name
        public int? GenreId { get; set; }
        public string? GenreName { get; set; }

        public int? PublisherId { get; set; }
        public int? PublishYear { get; set; }
        public string? AvailabilityStatus { get; set; }
    }
}
