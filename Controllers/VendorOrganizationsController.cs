using JobPortal.Data;
using JobPortal.Models.EntityModels;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortal.Controllers
{
    public class VendorOrganizationsController : Controller
    {
        private readonly JobPortalContext _db;

        public VendorOrganizationsController(JobPortalContext db)
        {
            _db = db;
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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var entityVendorOrg = new VendorOrganizations
            {
                VendorName = model.VendorName,
                VendorAddress = model.VendorAddress,
                VendorContact = model.VendorContact,
                VendorEmail = model.VendorEmail,
                VendorImage = model.VendorImage,
                CreatedBy = userId,
                CreatedDate = DateTime.Now
            };

            _db.Entry(entityVendorOrg).State = EntityState.Added;
            _db.SaveChanges();

            return RedirectToAction("Index");

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

        public IActionResult Edit(VendororganizationVM model)
        {
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
