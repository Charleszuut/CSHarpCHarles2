using Microsoft.AspNetCore.Identity;

namespace CSHarpCHarles2.Data
{
    public static class IdentitySeeder
    {
        private static readonly string[] Roles =
        {
            "SuperAdmin",
            "ProjectManager",
            "TeamMember",
            "FinanceOfficer"
        };

        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            const string adminEmail = "admin@csharpcharles.com";
            const string adminPassword = "admin123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (!createResult.Succeeded)
                {
                    var errors = string.Join(", ", createResult.Errors.Select(error => error.Description));
                    throw new InvalidOperationException($"Failed to create SuperAdmin user: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, "SuperAdmin"))
            {
                await userManager.AddToRoleAsync(adminUser, "SuperAdmin");
            }
        }
    }
}
