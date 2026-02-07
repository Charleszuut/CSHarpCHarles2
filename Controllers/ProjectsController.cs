using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CSHarpCHarles2.Models;

namespace CSHarpCHarles2.Controllers
{
    [Authorize(Roles = "SuperAdmin,ProjectManager")]
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var projects = new Dictionary<string, ProjectDetailsViewModel>(StringComparer.OrdinalIgnoreCase)
            {
                ["ecommerce-platform-redesign"] = new ProjectDetailsViewModel
                {
                    Id = "ecommerce-platform-redesign",
                    Title = "E-Commerce Platform Redesign",
                    Description = "Complete overhaul of the customer-facing e-commerce platform with improved UX and performance optimizations.",
                    Status = "In Progress",
                    Priority = "High",
                    ProgressPercent = 62,
                    StartDate = new DateOnly(2024, 1, 15),
                    EndDate = new DateOnly(2024, 6, 30),
                    BudgetSpent = 145000,
                    BudgetTotal = 250000,
                    ProjectManagerName = "Sarah Johnson",
                    ProjectManagerRole = "Lead Product Manager",
                    ProjectManagerInitials = "SJ",
                    AssignedTasks = new[]
                    {
                        new ProjectTask { Title = "Finalize UX wireframes", AssignedTo = "Emily Zhang" },
                        new ProjectTask { Title = "Implement checkout optimizations", AssignedTo = "Michael Torres" },
                        new ProjectTask { Title = "Integrate payment gateway", AssignedTo = "Priya Nair" },
                        new ProjectTask { Title = "Performance testing and monitoring", AssignedTo = "James Liu" }
                    }
                },
                ["mobile-app-development"] = new ProjectDetailsViewModel
                {
                    Id = "mobile-app-development",
                    Title = "Mobile App Development",
                    Description = "Native mobile application for iOS and Android platforms.",
                    Status = "In Progress",
                    Priority = "Critical",
                    ProgressPercent = 45,
                    StartDate = new DateOnly(2024, 2, 1),
                    EndDate = new DateOnly(2024, 8, 15),
                    BudgetSpent = 120000,
                    BudgetTotal = 350000,
                    ProjectManagerName = "David Chen",
                    ProjectManagerRole = "Mobile Engineering",
                    ProjectManagerInitials = "DC",
                    AssignedTasks = new[]
                    {
                        new ProjectTask { Title = "Implement authentication flow", AssignedTo = "Kevin Park" },
                        new ProjectTask { Title = "Build offline sync", AssignedTo = "Sofia Martinez" },
                        new ProjectTask { Title = "Push notification setup", AssignedTo = "Ryan O'Brien" },
                        new ProjectTask { Title = "Beta release preparation", AssignedTo = "Aisha Khan" }
                    }
                },
                ["api-gateway-implementation"] = new ProjectDetailsViewModel
                {
                    Id = "api-gateway-implementation",
                    Title = "API Gateway Implementation",
                    Description = "Centralized API gateway for microservices architecture.",
                    Status = "Planning",
                    Priority = "Medium",
                    ProgressPercent = 15,
                    StartDate = new DateOnly(2024, 4, 1),
                    EndDate = new DateOnly(2024, 9, 30),
                    BudgetSpent = 15000,
                    BudgetTotal = 180000,
                    ProjectManagerName = "Lisa Anderson",
                    ProjectManagerRole = "Platform Architect",
                    ProjectManagerInitials = "LA",
                    AssignedTasks = new[]
                    {
                        new ProjectTask { Title = "Define routing strategy", AssignedTo = "Tom Weber" },
                        new ProjectTask { Title = "Decide auth & rate limiting", AssignedTo = "Lena Petrov" },
                        new ProjectTask { Title = "Create gateway POC", AssignedTo = "Carlos Ruiz" }
                    }
                }
            };

            if (!projects.TryGetValue(id, out var project))
            {
                return NotFound();
            }

            return View(project);
        }
    }
}
