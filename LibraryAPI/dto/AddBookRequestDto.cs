using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.dto
{
    public class AddBookRequestDto
    {
        public string? Isbn { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string? Title { get; set; }

        [Required, Range(1,999, ErrorMessage = "AuthorId is required.")]
        public int? AuthorId { get; set; }
        [Required, Range(1,999, ErrorMessage = "PublisherId required")]
        public int? PublisherId { get; set; }

        [Range(0, 9999, ErrorMessage = "Invalid PublishYear. Year must be between 0 and 9999.")]
        public int? PublishYear { get; set; }

        [Required, Range(0, 999, ErrorMessage = "GenreId is required.")]
        public int? GenreId { get; set; }
    }
}
