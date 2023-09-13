using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.dto
{
    public class AddGenreRequestDto
    {
        [Required(ErrorMessage = "GenreName is required.")]
        [StringLength(100, ErrorMessage = "GenreName cannot exceed 100 characters.")]
        public string GenreName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}
