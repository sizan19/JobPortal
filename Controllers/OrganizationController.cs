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
            _db.Entry(entityorg).State = EntityState.Added; //Add the entity to the database
            _db.SaveChanges(); //Save the changes to the database
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]

        public IActionResult Edit(OrganizationVM model)
        {
            return View();
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
