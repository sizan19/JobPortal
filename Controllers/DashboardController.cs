using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
