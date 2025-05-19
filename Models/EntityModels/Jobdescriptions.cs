using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models.EntityModels
{
    public class Jobdescriptions : Common
    {
        [Key]
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







        [Required]

        public int CategoryId { get; set; }

        [Required]
        public int VendorId { get; set; }

        [ForeignKey("CategoryId")]
        public Categories Categories { get; set; }

        [ForeignKey("VendorId")]

        public VendorOrganizations VendorOrganizations { get; set; }

    }
}
