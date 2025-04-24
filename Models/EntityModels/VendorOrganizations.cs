using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortal.Models.EntityModels
{
    public class VendorOrganizations:Common
    {
        [Key]
        public int VendorId { get; set; }
        public string? VendorName { get; set; }
        public string? VendorAddress { get; set; }
        public string? VendorContact { get; set; }
        public string? VendorEmail { get; set; }

        //[Required]

        //public int CategoryId { get; set; }

        //[ForeignKey("CategoryId")]
        //public Categories Categories{ get; set; }




    }
}
