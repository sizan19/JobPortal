using JobPortal.Models.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Data;

public class JobPortalContext : IdentityDbContext<IdentityUser>
{
    public JobPortalContext(DbContextOptions<JobPortalContext> options)
        : base(options)
    {
    }


    public DbSet<Organization> Organization { get; set; }
    public DbSet<Categories> Categories { get; set; }

    public DbSet<VendorOrganizations> VendorOrganizations { get; set; }

    public DbSet<Jobdescriptions> jobdescriptions { get; set; }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
