using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSHarpCHarles2.Controllers
{
    [Authorize(Roles = "SuperAdmin,FinanceOfficer")]
    public class BudgetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
