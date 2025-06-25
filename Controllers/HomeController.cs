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

        // Controllers/HomeController.cs
        public IActionResult Index()
        {
            var jobs = (from i in _db.jobdescriptions
                        join j in _db.Categories on i.CategoryId equals j.CategoryId
                        join k in _db.VendorOrganizations on i.VendorId equals k.VendorId
                        where i.DeltetedDate == null
                        orderby i.DeadlineDate descending
                        select new JobdescriptionVM
                        {
                            JobId = i.JobId,
                            VendorId = i.VendorId,
                            CategoryId = i.CategoryId,
                            JobPositions = i.JobPositions,
                            JobVacancy = i.JobVacancy,
                            JobType = i.JobType,
                            Location = i.Location,
                            MinSalary = i.MinSalary,
                            MaxSalary = i.MaxSalary,
                            Experience = i.Experience,
                            DeadlineDate = i.DeadlineDate,
                            Description = i.Description,
                            VendorName = k.VendorName,
                            CategoryName = j.CategoryName,
                            VendorImage = k.VendorImage   // <-- Make sure this matches your DB property!
                        }).Take(10).ToList(); // Show latest 10 jobs

            return View(jobs);
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
