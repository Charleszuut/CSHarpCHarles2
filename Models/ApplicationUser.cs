using Microsoft.AspNetCore.Identity;

namespace CSHarpCHarles2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }

        public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active;
    }
}
