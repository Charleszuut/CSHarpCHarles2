namespace CSHarpCHarles2.Models
{
    public class UsersListItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public bool IsLockedOut { get; set; }
        public string RolesDisplay { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        public string DepartmentDisplay { get; set; } = string.Empty;

        public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active;
    }
}
