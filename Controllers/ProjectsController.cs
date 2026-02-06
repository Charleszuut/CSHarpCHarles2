using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSHarpCHarles2.Controllers
{
    [Authorize(Roles = "SuperAdmin,ProjectManager")]
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
