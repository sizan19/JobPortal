using JobPortal.Data;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

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


            return View();
        }

        //If empty , it will be a GET request

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(OrganizationVM model)
        {
            return View();
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
