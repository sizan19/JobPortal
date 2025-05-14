using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobPortal.Models.ViewModels
{
    public class VendororganizationVM
    {
        public int VendorId { get; set; }
        public string? VendorName { get; set; }
        public string? VendorAddress { get; set; }
        public string? VendorContact { get; set; }
        public string? VendorEmail { get; set; }

        public string? VendorImage { get; set; }


        public List<VendororganizationVM> VendororganizationList { get; set; }

        public VendororganizationVM()
        {
            VendororganizationList = new List<VendororganizationVM>();
        }
    }
}
