using JobPortal.Data;
using JobPortal.Models.EntityModels;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                                            VendorId = i.VendorId,
                                            CategoryId = i.CategoryId,
                                            JobPositions = i.JobPositions,
                                            JobVacancy = i.JobVacancy,
                                            JobType = i.JobType,
                                            Location = i.Location,
                                            MinSalary = i.MinSalary,
                                            MaxSalary = i.MaxSalary,
                                            Description = i.Description,
                                            VendorName = k.VendorName,
                                            CategoryName = j.CategoryName,
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
            ViewBag.VendorOrganizations = Utilities.CommonUtilities.GetVendorOrganizationList(_serviceProvider);
            ViewBag.Categories = Utilities.CommonUtilities.GetCategoryList(_serviceProvider);
            return View();
        }

        [HttpPost]


        public IActionResult Create(JobdescriptionVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Jobdescriptions entityJobDescription = new Jobdescriptions();
            entityJobDescription.VendorId = model.VendorId;
            entityJobDescription.CategoryId = model.CategoryId;
            entityJobDescription.JobPositions = model.JobPositions;
            entityJobDescription.JobVacancy = model.JobVacancy;
            entityJobDescription.JobType = model.JobType;
            entityJobDescription.Location = model.Location;
            entityJobDescription.MinSalary = model.MinSalary;
            entityJobDescription.MaxSalary = model.MaxSalary;
            entityJobDescription.Description = model.Description;
            entityJobDescription.CreatedBy = userId;
            entityJobDescription.CreatedDate = DateTime.Now;
            _db.Entry(entityJobDescription).State = EntityState.Added;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Edit(int id)
        {
            JobdescriptionVM model = new JobdescriptionVM();
            var org = _db.jobdescriptions
                .Where(x => x.JobId == id)
                .FirstOrDefault();
            if (org != null)
            {
                model.JobId = org.JobId;
                model.VendorId = org.VendorId;
                model.CategoryId = org.CategoryId;
                model.JobPositions = org.JobPositions;
                model.JobVacancy = org.JobVacancy;
                model.JobType = org.JobType;
                model.Location = org.Location;
                model.MinSalary = org.MinSalary;
                model.MaxSalary = org.MaxSalary;
                model.Description = org.Description;
            }
            ViewBag.VendorOrganizations = Utilities.CommonUtilities.GetVendorOrganizationList(_serviceProvider);
            ViewBag.Categories = Utilities.CommonUtilities.GetCategoryList(_serviceProvider);
            return View(model);
        }

        [HttpPost]

        public IActionResult Edit(JobdescriptionVM model)
        {
            ViewBag.VendorOrgaization = Utilities.CommonUtilities.GetVendorOrganizationList(_serviceProvider);
            ViewBag.Category = Utilities.CommonUtilities.GetCategoryList(_serviceProvider);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Jobdescriptions entityJobDescription = new Jobdescriptions();
            entityJobDescription.JobId = model.JobId;
            entityJobDescription.VendorId = model.VendorId;
            entityJobDescription.CategoryId = model.CategoryId;
            entityJobDescription.JobPositions = model.JobPositions;
            entityJobDescription.JobVacancy = model.JobVacancy;
            entityJobDescription.JobType = model.JobType;
            entityJobDescription.Location = model.Location;
            entityJobDescription.MinSalary = model.MinSalary;
            entityJobDescription.MaxSalary = model.MaxSalary;
            entityJobDescription.Description = model.Description;
            entityJobDescription.UpdatedBy = userId;
            entityJobDescription.UpdatedDate = DateTime.Now;
            _db.Entry(entityJobDescription).State = EntityState.Modified;
            _db.Entry(entityJobDescription)
                .Property(x => x.CreatedBy)
                .IsModified = false;
            _db.Entry(entityJobDescription)
                .Property(x => x.CreatedDate)
                .IsModified = false;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var data = _db.jobdescriptions
                .Where(x => x.JobId == id)
                .FirstOrDefault();
            if (data != null)
            {
                data.DeltetedBy = userId;
                data.DeltetedDate = DateTime.Now;
                _db.Entry(data).State = EntityState.Modified;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");

        }
    }
}
