using JobPortal.Data;
using JobPortal.Models.EntityModels;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JobPortal.Controllers
{
    [Authorize]
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

            // Base query to get all job listings with related data
            var baseQuery = from i in _db.jobdescriptions
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
                                Experience = i.Experience,
                                DeadlineDate = i.DeadlineDate,
                                Description = i.Description,
                                VendorName = k.VendorName,
                                CategoryName = j.CategoryName
                            };

            // Role-based filtering
            if (User.IsInRole("Vendor"))
            {
                int vendorId = _db.VendorOrganizations
                    .Where(x => x.VendorEmail == User.FindFirstValue(ClaimTypes.Email))
                    .Select(x => x.VendorId)
                    .FirstOrDefault();

                if (vendorId > 0)
                {
                    model.JobdescriptionList = baseQuery
                        .Where(x => x.VendorId == vendorId)
                        .ToList();
                }
                else
                {
                    model.JobdescriptionList = new List<JobdescriptionVM>();
                }

                ViewBag.IsVendorView = true;
                ViewBag.VendorName = _db.VendorOrganizations
                    .Where(x => x.VendorEmail == User.FindFirstValue(ClaimTypes.Email))
                    .Select(x => x.VendorName)
                    .FirstOrDefault();
            }
            else if (User.IsInRole("Admin") || User.IsInRole("admin"))
            {
                model.JobdescriptionList = baseQuery
                    .ToList();

                ViewBag.IsVendorView = false;
                ViewBag.TotalJobListings = model.JobdescriptionList.Count;
            }
            else
            {
                model.JobdescriptionList = new List<JobdescriptionVM>();
                ViewBag.IsVendorView = false;
            }

            return View(model);
        }

        public IActionResult Create()
        {
            JobdescriptionVM model = new JobdescriptionVM();

            // Auto-fill vendor information for logged-in vendors
            if (User.IsInRole("Vendor"))
            {
                var vendorInfo = _db.VendorOrganizations
                    .Where(x => x.VendorEmail == User.FindFirstValue(ClaimTypes.Email))
                    .FirstOrDefault();

                if (vendorInfo != null)
                {
                    model.VendorId = vendorInfo.VendorId;
                    model.VendorName = vendorInfo.VendorName;
                    ViewBag.IsVendorUser = true;
                }
            }
            else
            {
                ViewBag.IsVendorUser = false;
            }

            ViewBag.VendorOrganizations = Utilities.CommonUtilities.GetVendorOrganizationList(_serviceProvider);
            ViewBag.Categories = Utilities.CommonUtilities.GetCategoryList(_serviceProvider);
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(JobdescriptionVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Validation: Vendors can only create jobs under their own organization
            if (User.IsInRole("Vendor"))
            {
                var vendorId = _db.VendorOrganizations
                    .Where(x => x.VendorEmail == User.FindFirstValue(ClaimTypes.Email))
                    .Select(x => x.VendorId)
                    .FirstOrDefault();

                if (vendorId == 0 || model.VendorId != vendorId)
                {
                    ModelState.AddModelError("", "You can only create jobs for your own organization.");
                    ViewBag.VendorOrganizations = Utilities.CommonUtilities.GetVendorOrganizationList(_serviceProvider);
                    ViewBag.Categories = Utilities.CommonUtilities.GetCategoryList(_serviceProvider);
                    ViewBag.IsVendorUser = true;
                    return View(model);
                }
            }

            if (ModelState.IsValid)
            {
                Jobdescriptions entityJobDescription = new Jobdescriptions();
                entityJobDescription.VendorId = model.VendorId;
                entityJobDescription.CategoryId = model.CategoryId;
                entityJobDescription.JobPositions = model.JobPositions;
                entityJobDescription.JobVacancy = model.JobVacancy;
                entityJobDescription.JobType = model.JobType;
                entityJobDescription.Location = model.Location;
                entityJobDescription.MinSalary = model.MinSalary;
                entityJobDescription.MaxSalary = model.MaxSalary;
                entityJobDescription.Experience = model.Experience;
                entityJobDescription.DeadlineDate = model.DeadlineDate;
                entityJobDescription.Description = model.Description;
                entityJobDescription.CreatedBy = userId;
                entityJobDescription.CreatedDate = DateTime.Now;

                _db.Entry(entityJobDescription).State = EntityState.Added;
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Job listing created successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.VendorOrganizations = Utilities.CommonUtilities.GetVendorOrganizationList(_serviceProvider);
            ViewBag.Categories = Utilities.CommonUtilities.GetCategoryList(_serviceProvider);
            ViewBag.IsVendorUser = User.IsInRole("Vendor");
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var jobListing = _db.jobdescriptions
                .Where(x => x.JobId == id && x.DeltetedDate == null)
                .FirstOrDefault();

            if (jobListing == null)
            {
                TempData["ErrorMessage"] = "Job listing not found.";
                return RedirectToAction("Index");
            }

            // Security check: Vendors can only edit their own jobs
            if (User.IsInRole("Vendor"))
            {
                var vendorId = _db.VendorOrganizations
                    .Where(x => x.VendorEmail == User.FindFirstValue(ClaimTypes.Email))
                    .Select(x => x.VendorId)
                    .FirstOrDefault();

                if (jobListing.VendorId != vendorId)
                {
                    TempData["ErrorMessage"] = "You can only edit your own job listings.";
                    return RedirectToAction("Index");
                }
            }

            JobdescriptionVM model = new JobdescriptionVM();
            model.JobId = jobListing.JobId;
            model.VendorId = jobListing.VendorId;
            model.CategoryId = jobListing.CategoryId;
            model.JobPositions = jobListing.JobPositions;
            model.JobVacancy = jobListing.JobVacancy;
            model.JobType = jobListing.JobType;
            model.Location = jobListing.Location;
            model.MinSalary = jobListing.MinSalary;
            model.MaxSalary = jobListing.MaxSalary;
            model.Experience = jobListing.Experience;
            model.DeadlineDate = jobListing.DeadlineDate;
            model.Description = jobListing.Description;

            ViewBag.VendorOrganizations = Utilities.CommonUtilities.GetVendorOrganizationList(_serviceProvider);
            ViewBag.Categories = Utilities.CommonUtilities.GetCategoryList(_serviceProvider);
            ViewBag.IsVendorUser = User.IsInRole("Vendor");

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(JobdescriptionVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Security check: Vendors can only edit their own jobs
            if (User.IsInRole("Vendor"))
            {
                var vendorId = _db.VendorOrganizations
                    .Where(x => x.VendorEmail == User.FindFirstValue(ClaimTypes.Email))
                    .Select(x => x.VendorId)
                    .FirstOrDefault();

                if (model.VendorId != vendorId)
                {
                    TempData["ErrorMessage"] = "You can only edit your own job listings.";
                    return RedirectToAction("Index");
                }
            }

            if (ModelState.IsValid)
            {
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
                entityJobDescription.Experience = model.Experience;
                entityJobDescription.DeadlineDate = model.DeadlineDate;
                entityJobDescription.Description = model.Description;
                entityJobDescription.UpdatedBy = userId;
                entityJobDescription.UpdatedDate = DateTime.Now;

                _db.Entry(entityJobDescription).State = EntityState.Modified;
                _db.Entry(entityJobDescription).Property(x => x.CreatedBy).IsModified = false;
                _db.Entry(entityJobDescription).Property(x => x.CreatedDate).IsModified = false;
                _db.SaveChanges();

                TempData["SuccessMessage"] = "Job listing updated successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.VendorOrganizations = Utilities.CommonUtilities.GetVendorOrganizationList(_serviceProvider);
            ViewBag.Categories = Utilities.CommonUtilities.GetCategoryList(_serviceProvider);
            ViewBag.IsVendorUser = User.IsInRole("Vendor");
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var jobListing = _db.jobdescriptions
                .Where(x => x.JobId == id && x.DeltetedDate == null)
                .FirstOrDefault();

            if (jobListing == null)
            {
                TempData["ErrorMessage"] = "Job listing not found.";
                return RedirectToAction("Index");
            }

            // Security check: Vendors can only delete their own jobs
            if (User.IsInRole("Vendor"))
            {
                var vendorId = _db.VendorOrganizations
                    .Where(x => x.VendorEmail == User.FindFirstValue(ClaimTypes.Email))
                    .Select(x => x.VendorId)
                    .FirstOrDefault();

                if (jobListing.VendorId != vendorId)
                {
                    TempData["ErrorMessage"] = "You can only delete your own job listings.";
                    return RedirectToAction("Index");
                }
            }

            jobListing.DeltetedBy = userId;
            jobListing.DeltetedDate = DateTime.Now;
            _db.Entry(jobListing).State = EntityState.Modified;
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Job listing deleted successfully!";
            return RedirectToAction("Index");
        }

        // Additional method for admins to view all job statistics
        [Authorize(Roles = "Admin,admin")]
        public IActionResult Statistics()
        {
            var stats = new
            {
                TotalJobs = _db.jobdescriptions.Count(x => x.DeltetedDate == null),
                ActiveJobs = _db.jobdescriptions.Count(x => x.DeltetedDate == null && x.DeadlineDate >= DateTime.Now),
                ExpiredJobs = _db.jobdescriptions.Count(x => x.DeltetedDate == null && x.DeadlineDate < DateTime.Now),
                TotalVendors = _db.VendorOrganizations.Count(x => x.DeltetedDate == null),
                JobsByCategory = _db.jobdescriptions
                    .Where(x => x.DeltetedDate == null)
                    .Join(_db.Categories, j => j.CategoryId, c => c.CategoryId, (j, c) => new { c.CategoryName })
                    .GroupBy(x => x.CategoryName)
                    .Select(g => new { Category = g.Key, Count = g.Count() })
                    .ToList()
            };

            ViewBag.Statistics = stats;
            return View();
        }


        public IActionResult Details(int id)
        {
            var job = (from i in _db.jobdescriptions
                       join j in _db.Categories on i.CategoryId equals j.CategoryId
                       join k in _db.VendorOrganizations on i.VendorId equals k.VendorId
                       where i.JobId == id
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
                           VendorImage = k.VendorImage,
                           CategoryName = j.CategoryName
                       }).FirstOrDefault();

            if (job == null)
                return NotFound();

            return View(job);
        }
    }
}