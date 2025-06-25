using System;
using System.Collections.Generic;

namespace JobPortal.Models.ViewModels
{
    public class DashboardVM
    {
        public int TotalJobs { get; set; }
        public int ActiveJobs { get; set; }
        public int ExpiredJobs { get; set; }
        public int TotalApplications { get; set; }
        public List<JobdescriptionVM> RecentJobs { get; set; }
        public string UserType { get; set; } // "Admin" or "Vendor"
    }
}