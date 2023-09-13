using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.dto
{
    public class AddAuthorRequestDto
    {
        [Required(ErrorMessage = "FirstName is required.")]
        [StringLength(50, ErrorMessage = "FirstName cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required.")]
        [StringLength(50, ErrorMessage = "LastName cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "Nationality cannot exceed 50 characters.")]
        public string Nationality { get; set; }
    }
}
