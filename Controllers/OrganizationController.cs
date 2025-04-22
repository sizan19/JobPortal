using JobPortal.Data;
using JobPortal.Models.EntityModels;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortal.Controllers


{
    [Authorize]
    public class OrganizationController : Controller
    {

        private readonly JobPortalContext _db;

        public OrganizationController(JobPortalContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {

            OrganizationVM model = new OrganizationVM();
            model.OrganizationList = (from i in _db.Organization    //SElECT * FROM Organization
                                      where i.DeltetedDate == null
                                      select new OrganizationVM 
                                      {
                                          OrganizationId = i.OrganizationId,   //converting from Entitymdoel to viewmodel
                                          OrgName = i.OrgName,
                                          OrgAddress = i.OrgAddress,
                                          OrgContact = i.OrgContact,
                                          OrgEmail = i.OrgEmail,
                                          OrgImage = i.OrgImage,
                                      }).ToList();
            return View(model);

            
        }

        //If empty , it will be a GET request

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(OrganizationVM model, IFormFile? file)
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
                model.OrgImage = $"/Files/{file.FileName}";
            }

            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Organization entityorg = new Organization();
            entityorg.OrgName = model.OrgName;
            entityorg.OrgAddress = model.OrgAddress;
            entityorg.OrgContact = model.OrgContact;
            entityorg.OrgEmail = model.OrgEmail;
            entityorg.CreatedBy = UserId; //Set the created by field
            entityorg.CreatedDate = DateTime.Now; //Set the created date field
            entityorg.OrgImage = model.OrgImage;    
            _db.Entry(entityorg).State = EntityState.Added; //Add the entity to the database
            _db.SaveChanges(); //Save the changes to the database
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var data = _db.Organization.FirstOrDefault(x => x.OrganizationId == id);
            if (data == null)
            {
                return NotFound();
            }

            OrganizationVM model = new OrganizationVM
            {
                OrganizationId = data.OrganizationId,
                OrgName = data.OrgName,
                OrgAddress = data.OrgAddress,
                OrgContact = data.OrgContact,
                OrgEmail = data.OrgEmail,
                OrgImage = data.OrgImage
            };

            return View(model);
        }

        [HttpPost]

        public IActionResult Edit(OrganizationVM model, IFormFile? file)
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
                model.OrgImage = $"/Files/{file.FileName}";
            }
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Organization entityorg = new Organization();
            entityorg.OrganizationId = model.OrganizationId;
            entityorg.OrgName = model.OrgName;
            entityorg.OrgAddress = model.OrgAddress;
            entityorg.OrgContact = model.OrgContact;
            entityorg.OrgEmail = model.OrgEmail;
            entityorg.OrgImage = model.OrgImage;
            entityorg.UpdatedBy = UserId; //Set the created by field
            entityorg.UpdatedDate = DateTime.Now; //Set the created date field
            _db.Entry(entityorg).State = EntityState.Modified; //Add the entity to the database
            _db.Entry(entityorg).Property(x => x.CreatedBy).IsModified = false;  // so that these values were removed when updating database
            _db.Entry(entityorg).Property(x => x.CreatedDate).IsModified = false;

            if(file == null)
            {
                _db.Entry(entityorg).Property(x => x.OrgImage).IsModified = false; // so that these values were removed when updating database

            }

            _db.SaveChanges(); //Save the changes to the database
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            var data = _db.Organization.FirstOrDefault(x => x.OrganizationId == id);
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

        //[HttpPost]
        //public IActionResult Delete(OrganizationVM model)  --there is no need of view
        //{
        //    return View();
        //}
    }
}
