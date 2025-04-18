using JobPortal.Data;
using JobPortal.Models.EntityModels;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Controllers
{
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
                                      select new OrganizationVM
                                      {
                                          OrganizationId = i.OrganizationId,   //converting from Entitymdoel to viewmodel
                                          OrgName = i.OrgName,
                                          OrgAddress = i.OrgAddress,
                                          OrgContact = i.OrgContact,
                                          OrgEmail = i.OrgEmail,
                                      }).ToList();
            return View(model);

            
        }

        //If empty , it will be a GET request

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(OrganizationVM model)
        {
            Organization entityorg = new Organization();
            entityorg.OrgName = model.OrgName;
            entityorg.OrgAddress = model.OrgAddress;
            entityorg.OrgContact = model.OrgContact;
            entityorg.OrgEmail = model.OrgEmail;
            entityorg.CreatedBy = "Admin"; //Set the created by field
            entityorg.CreatedDate = DateTime.Now; //Set the created date field
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
                OrgEmail = data.OrgEmail
            };

            return View(model);
        }

        [HttpPost]

        public IActionResult Edit(OrganizationVM model)
        {
            Organization entityorg = new Organization();
            entityorg.OrganizationId = model.OrganizationId;
            entityorg.OrgName = model.OrgName;
            entityorg.OrgAddress = model.OrgAddress;
            entityorg.OrgContact = model.OrgContact;
            entityorg.OrgEmail = model.OrgEmail;
            entityorg.UpdatedBy = "Admin"; //Set the created by field
            entityorg.CreatedDate = DateTime.Now; //Set the created date field
            _db.Entry(entityorg).State = EntityState.Modified; //Add the entity to the database
            _db.SaveChanges(); //Save the changes to the database
            return RedirectToAction("Index");
        }


        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(OrganizationVM model)
        {
            return View();
        }
    }
}
