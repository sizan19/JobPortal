using System.Diagnostics;
using System.Security.Claims;
using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly JobPortalContext _db;

        public HomeController(ILogger<HomeController> logger, JobPortalContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            OrganizationVM model = new OrganizationVM();
            model.OrganizationList = (from i in _db.Organization    //SElECT * FROM Organization
                                      where i.DeltetedDate == null
                                      select new OrganizationVM
                                      {
                                          OrganizationId = i.OrganizationId,   //converting from Entitymdoel to viewmodel
                                          OrgName = i.OrgName,
                                          OrgAddress = i.OrgAddress,
                                          OrgContact = i.OrgContact,
                                          OrgEmail = i.OrgEmail,
                                          OrgImage = i.OrgImage,
                                      }).ToList();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
