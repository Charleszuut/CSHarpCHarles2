using CSHarpCHarles2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSHarpCHarles2.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private static string HumanizeRoleName(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return string.Empty;
            }

            if (role.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase))
            {
                return "Super Admin";
            }

            var result = new List<char>(role.Length + 6);
            for (var i = 0; i < role.Length; i++)
            {
                var c = role[i];
                if (i > 0 && char.IsUpper(c) && char.IsLower(role[i - 1]))
                {
                    result.Add(' ');
                }

                result.Add(c);
            }

            return new string(result.ToArray());
        }

        private static string DepartmentFromRoles(IList<string> roles)
        {
            if (roles == null || roles.Count == 0)
            {
                return "-";
            }

            var primary = roles.OrderBy(r => r).First();

            return primary switch
            {
                "SuperAdmin" => "Administration",
                "ProjectManager" => "Project Management",
                "TeamMember" => "Project Team",
                "FinanceOfficer" => "Finance",
                _ => HumanizeRoleName(primary)
            };
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.Email)
                .ToListAsync();

            var items = new List<UsersListItemViewModel>(users.Count);
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var rolesOrdered = roles.OrderBy(r => r).ToList();
                items.Add(new UsersListItemViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    EmailConfirmed = user.EmailConfirmed,
                    IsLockedOut = user.LockoutEnd.HasValue,
                    RolesDisplay = rolesOrdered.Count == 0 ? "-" : string.Join(", ", rolesOrdered.Select(HumanizeRoleName)),
                    DisplayName = user.DisplayName ?? user.Email ?? string.Empty,
                    DepartmentDisplay = DepartmentFromRoles(rolesOrdered),
                    EmploymentStatus = user.EmploymentStatus
                });
            }

            return View(items);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");

            return View(new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                DisplayName = user.DisplayName ?? string.Empty,
                EmploymentStatus = user.EmploymentStatus,
                IsSuperAdmin = isSuperAdmin
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");
            model.IsSuperAdmin = isSuperAdmin;

            var emailResult = await _userManager.SetEmailAsync(user, model.Email);
            if (!emailResult.Succeeded)
            {
                foreach (var error in emailResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            var userNameResult = await _userManager.SetUserNameAsync(user, model.Email);
            if (!userNameResult.Succeeded)
            {
                foreach (var error in userNameResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            user.DisplayName = model.DisplayName;

            if (!isSuperAdmin)
            {
                user.EmploymentStatus = model.EmploymentStatus;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                model.Email = user.Email ?? string.Empty;
                return View(model);
            }

            TempData["StatusMessage"] = "User updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var allowedRoles = new[] { "ProjectManager", "TeamMember", "FinanceOfficer" };
            if (!allowedRoles.Contains(model.Role))
            {
                ModelState.AddModelError(nameof(model.Role), "Invalid role.");
                return View(model);
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError(string.Empty, "Role does not exist. Please contact the system administrator.");
                return View(model);
            }

            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null)
            {
                ModelState.AddModelError(nameof(model.Email), "A user with this email already exists.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                DisplayName = model.DisplayName
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                await _userManager.DeleteAsync(user);
                return View(model);
            }

            TempData["StatusMessage"] = "User account created successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(new ResetPasswordViewModel
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                model.Email = user.Email ?? string.Empty;
                return View(model);
            }

            TempData["StatusMessage"] = "Password reset successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
