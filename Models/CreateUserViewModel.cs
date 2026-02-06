using System.ComponentModel.DataAnnotations;

namespace CSHarpCHarles2.Models
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Department Role")]
        public string Role { get; set; } = string.Empty;
    }
}
