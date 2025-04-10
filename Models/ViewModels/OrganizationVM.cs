namespace JobPortal.Models.ViewModels
{
    public class OrganizationVM
    {
        public int OrganizationId { get; set; }
        public string OrgName { get; set; }
        public string OrgAddress { get; set; }
        public int OrgContact { get; set; }
        public string? OrgEmail { get; set; }

        public string? OrgImage { get; set; }
    }
}
