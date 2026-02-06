using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSHarpCHarles2.Controllers
{
    [Authorize(Roles = "SuperAdmin,ProjectManager,TeamMember")]
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
