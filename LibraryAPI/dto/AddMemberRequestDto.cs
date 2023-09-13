using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.dto
{
    public class AddMemberRequestDto
    {
        [Required(ErrorMessage = "FirstName is required.")]
        [StringLength(50, ErrorMessage = "FirstName cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        [StringLength(50, ErrorMessage = "LastName cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid PhoneNumber format.")]
        public string PhoneNumber { get; set; }

        [StringLength(20, ErrorMessage = "MembershipStatus cannot exceed 20 characters.")]
        public string? MembershipStatus { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
