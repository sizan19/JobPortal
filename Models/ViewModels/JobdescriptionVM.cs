namespace JobPortal.Models.ViewModels
{
    public class JobdescriptionVM
    {

        public int JobId { get; set; }

        public string? Description { get; set; }

        public string? CompanyName { get; set; }

        public string? Location { get; set; }

        public string? JobType { get; set; } // e.g., Full-time, Part-time, Contract

        public string? JobPositions { get; set; }

        public int JobVacancy { get; set; }


        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }

        public int? Experience { get; set; }

        public DateTime? DeadlineDate { get; set; }

        public int CategoryId { get; set; }
        public int VendorId { get; set; }

        public string? VendorName { get; set; }
        public string? CategoryName { get; set; }


        public List<JobdescriptionVM> JobdescriptionList{ get; set; }

        public JobdescriptionVM()
        {
            JobdescriptionList = new List<JobdescriptionVM>();
        }


    }
}
