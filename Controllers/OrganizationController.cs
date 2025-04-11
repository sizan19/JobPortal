using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class OrganizationController : Controller
    {
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

        public IActionResult Edit()
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
