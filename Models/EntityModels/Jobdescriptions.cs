using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models.EntityModels
{
    public class Jobdescriptions
    {
        [Key]
        public int JobId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? CompanyName { get; set; }

        public string? Location { get; set; }

        public string? JobType { get; set; } // e.g., Full-time, Part-time, Contract

        public string? JobPositions { get; set; }

        public decimal? Salary { get; set; }

        public int VendorId { get; set; }

    }
}
