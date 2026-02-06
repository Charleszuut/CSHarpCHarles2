using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSHarpCHarles2.Controllers
{
    [Authorize(Roles = "SuperAdmin,ProjectManager")]
    public class ResourcesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
