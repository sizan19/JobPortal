using JobPortal.Data;
using JobPortal.Models.EntityModels;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobPortal.Controllers
{

    public class JobDescriptionController : Controller


    {
        private readonly JobPortalContext _db;
        private readonly IServiceProvider _serviceProvider;


        public JobDescriptionController(JobPortalContext db, IServiceProvider serviceProvider)
        {
            _db = db;
            _serviceProvider = serviceProvider;
        }

        public IActionResult Index()
        {
            JobdescriptionVM model = new JobdescriptionVM();
            model.JobdescriptionList = (from i in _db.jobdescriptions
                                        join j in _db.Categories on i.CategoryId equals j.CategoryId
                                        join k in _db.VendorOrganizations on i.VendorId equals k.VendorId
                                        where i.DeltetedDate == null
                                        select new JobdescriptionVM
                                        {
                                            JobId = i.JobId,
                                            Title = i.Title,
                                            Description = i.Description,
                                            CompanyName = i.CompanyName,
                                            Location = i.Location,
                                            JobType = i.JobType,
                                            JobPositions = i.JobPositions,
                                            JobVacancy = i.JobVacancy,
                                            MinSalary = i.MinSalary,
                                            MaxSalary = i.MaxSalary,
                                            CategoryId = i.CategoryId,
                                            VendorId = i.VendorId,
                                        }).ToList();
            

            if (User.IsInRole("Vendor"))
            {
                int vendorId = _db.VendorOrganizations
                .Where(x => x.VendorEmail == User.FindFirstValue(ClaimTypes.Email))
                .Select(x => x.VendorId)
                .FirstOrDefault();
                model.JobdescriptionList = model.JobdescriptionList.Where(x => x.VendorId == vendorId).ToList();
            }

            return View(model);
        }

        //If empty , it will be a GET request

        public IActionResult Create()
        {

            return View();

        }

        [HttpPost]


        public IActionResult Create(JobdescriptionVM model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var job = new Jobdescriptions
                {
                    Title = model.Title,
                    Description = model.Description,
                    CompanyName = model.CompanyName,
                    Location = model.Location,
                    JobType = model.JobType,
                    JobPositions = model.JobPositions,
                    MinSalary = model.MinSalary,
                    MaxSalary = model.MaxSalary,
                    // Optional if image path is relevant
                    // OrgImage = model.OrgImage
                };

                _db.jobdescriptions.Add(job);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }


        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]

        public IActionResult Edit(JobdescriptionVM model)
        {
            return View();
        }


        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(JobdescriptionVM model)
        {
            return View();
        }
    }
}
