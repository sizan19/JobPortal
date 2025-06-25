using JobPortal.Data;
using JobPortal.Models.EntityModels;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortal.Controllers
{
    [Authorize]
    public class VendorOrganizationsController : Controller
    {
        private readonly JobPortalContext _db;
        private readonly UserManager<IdentityUser> _userManager;


        public VendorOrganizationsController(JobPortalContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            VendororganizationVM model = new VendororganizationVM();
            model.VendororganizationList = (from i in _db.VendorOrganizations    //SElECT * FROM Vendor organization
                                            where i.DeltetedDate == null
                                            select new VendororganizationVM
                                    {
                                        VendorId = i.VendorId,   //converting from Entitymdoel to viewmodel
                                        VendorName = i.VendorName,
                                        VendorAddress = i.VendorAddress,
                                        VendorContact = i.VendorContact,
                                        VendorEmail = i.VendorEmail,
                                        VendorImage = i.VendorImage,
                                    }).ToList();
            return View(model);
        }

        //If empty , it will be a GET request

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(VendororganizationVM model, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (file != null)
            {
                var GuidId = Guid.NewGuid().ToString();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string fileName = GuidId + file.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                model.VendorImage = "/Files/" + GuidId + file.FileName;
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    VendorOrganizations org = new VendorOrganizations
                    {
                        VendorName = model.VendorName,
                        VendorAddress = model.VendorAddress,
                        VendorEmail = model.VendorEmail,
                        VendorContact = model.VendorContact,
                        VendorImage = model.VendorImage,
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now
                    };
                    _db.Entry(org).State = EntityState.Added;
                    _db.SaveChanges();

                    IdentityUser user = new IdentityUser
                    {
                        UserName = model.VendorEmail,
                        Email = model.VendorEmail,
                        //OrgId = org.VendorOrgId,
                        EmailConfirmed = true,
                    };
                    var result = _userManager.CreateAsync(user, $"Job@12{model.VendorEmail}").Result;
                    if (result.Succeeded)
                    {
                        _userManager.AddToRoleAsync(user, "Vendor").Wait();
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Failed to create user.");
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the organization.");
                }
            }
            return View(model);
        }



        public IActionResult Edit(int id)
        {
            var data = _db.VendorOrganizations.FirstOrDefault(x => x.VendorId == id);
            if (data == null)
            {
                return NotFound();
            }

            VendororganizationVM model = new VendororganizationVM
            {
                VendorId = data.VendorId,
                VendorName = data.VendorName,
                VendorAddress = data.VendorAddress,
                VendorContact = data.VendorContact,
                VendorEmail = data.VendorEmail,
                VendorImage = data.VendorImage
            };

            return View(model);

        }

        [HttpPost]

        public IActionResult Edit(VendororganizationVM model, IFormFile? file)
        {
            if (file != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string fileNameWithPath = Path.Combine(path, file.FileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                model.VendorImage = $"/Files/{file.FileName}";
            }
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            VendorOrganizations entitycat = new VendorOrganizations();
            entitycat.VendorId = model.VendorId;
            entitycat.VendorName = model.VendorName;
            entitycat.VendorAddress = model.VendorAddress;
            entitycat.VendorContact = model.VendorContact;
            entitycat.VendorEmail = model.VendorEmail;
            entitycat.VendorImage = model.VendorImage;

            entitycat.UpdatedBy = UserId; //Set the created by field
            entitycat.UpdatedDate = DateTime.Now; //Set the created date field
            _db.Entry(entitycat).State = EntityState.Modified; //Add the entity to the database
            _db.Entry(entitycat).Property(x => x.CreatedBy).IsModified = false;  // so that these values were removed when updating database
            _db.Entry(entitycat).Property(x => x.CreatedDate).IsModified = false;
            _db.SaveChanges(); //Save the changes to the database
            return RedirectToAction("Index");

        }


        public IActionResult Delete(int id)
        {
            var data = _db.VendorOrganizations.FirstOrDefault(x => x.VendorId == id);
            if (data == null)
            {
                return NotFound();
            }
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (data != null)
            {
                data.DeltetedBy = UserId; //Set the IsDeleted property to true
                data.DeltetedDate = DateTime.Now;
                _db.Entry(data).State = EntityState.Modified; //Add the entity to the database
                _db.SaveChanges(); //Save the changes to the database
            }


            return RedirectToAction("Index");

        }

    }
}
