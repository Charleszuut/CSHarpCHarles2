using System.ComponentModel.DataAnnotations;

namespace CSHarpCHarles2.Models
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Employment Status")]
        public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active;

        public bool IsSuperAdmin { get; set; }
    }
}
