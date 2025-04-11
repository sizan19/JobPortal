using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class OrganizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
