using JobPortal.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Utilities
{
    public static class CommonUtilities
    {
        public static IEnumerable<SelectListItem>
            GetOrganizationList(IServiceProvider serviceProvider)
        {
            var options = serviceProvider
                .GetRequiredService<DbContextOptions<JobPortalContext>>();

            using (var _ent = new JobPortalContext(options))
            {
                return _ent.Organization
                    .Where(x => x.DeltetedDate == null)
                    .Select(v => new SelectListItem
                    {
                        Value = v.OrganizationId.ToString(),
                        Text = v.OrgName
                    })
                    .ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetVendorOrganizationList(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<JobPortalContext>>();

            using (var _ent = new JobPortalContext(options))
            {
                return _ent.VendorOrganizations
                    .Where(x => x.DeltetedDate == null)
                    .Select(v => new SelectListItem
                    {
                        Value = v.VendorId.ToString(),
                        Text = v.VendorName
                    })
                    .ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetCategoryList(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<JobPortalContext>>();

            using (var _ent = new JobPortalContext(options))
            {
                return _ent.Categories
                    .Where(x => x.DeltetedDate == null)
                    .Select(v => new SelectListItem
                    {
                        Value = v.CategoryId.ToString(),
                        Text = v.CategoryName
                    })
                    .ToList();
            }
        }

        public static async Task<List<SelectListItem>> GetCategoryListAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<JobPortalContext>();

            return await context.Categories
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName})
                .ToListAsync();
        }



    }
}
