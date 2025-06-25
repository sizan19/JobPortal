using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using JobPortal.Models;
using JobPortal.Models.ViewModels;
using System.Collections.Generic;
using JobPortal.Models.EntityModels;
using JobPortal.Data;

namespace JobPortal.Controllers
{
    public class DashboardController : Controller
    {
        private readonly JobPortalContext _db;

        public DashboardController(JobPortalContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            string userType = User.IsInRole("Admin") ? "Admin" : "Vendor";
            int currentVendorId = 0;

            if (userType == "Vendor")
            {
                // Try get VendorId as int from claim or from DB
                var vendorIdStr = User.FindFirst("VendorId")?.Value;
                if (!int.TryParse(vendorIdStr, out currentVendorId))
                {
                    currentVendorId = _db.VendorOrganizations
                        .Where(v => v.VendorEmail == User.Identity.Name)
                        .Select(v => v.VendorId)
                        .FirstOrDefault();
                }
            }
            var today = DateTime.Now;

            IQueryable<Jobdescriptions> jobsQuery = _db.jobdescriptions.Where(j => j.DeltetedDate == null);
            if (userType == "Vendor" && currentVendorId != 0)
            {
                jobsQuery = jobsQuery.Where(j => j.VendorId == currentVendorId);
            }

            int totalJobs = jobsQuery.Count();
            int activeJobs = jobsQuery.Count(j => j.DeadlineDate >= today);
            int expiredJobs = jobsQuery.Count(j => j.DeadlineDate < today);

            // Only for Admin and only if Applications exist:
            int totalApplications = 0;
            // Remove or comment out this if you don't have Applications table
            // if (userType == "Admin" && _db.GetType().GetProperties().Any(p => p.Name == "Applications"))
            // {
            //     totalApplications = _db.Applications.Count();
            // }

            var recentJobs = (from job in jobsQuery
                              join vendor in _db.VendorOrganizations on job.VendorId equals vendor.VendorId
                              orderby job.DeadlineDate descending
                              select new JobdescriptionVM
                              {
                                  JobId = job.JobId,
                                  JobPositions = job.JobPositions,
                                  DeadlineDate = job.DeadlineDate,
                                  VendorName = vendor.VendorName,
                                  VendorImage = vendor.VendorImage // <-- Correct property name!
                              }).Take(5).ToList();

            var model = new DashboardVM
            {
                TotalJobs = totalJobs,
                ActiveJobs = activeJobs,
                ExpiredJobs = expiredJobs,
                TotalApplications = totalApplications,
                RecentJobs = recentJobs,
                UserType = userType
            };

            return View(model);
        }
    }
}