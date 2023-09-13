using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.dto
{
    public class AddBorrowRequestDto
    {
        [Required(ErrorMessage = "MemberId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "MemberId must be a positive integer.")]
        public int MemberId { get; set; }
    }
}
