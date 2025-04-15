using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JobPortal.Controllers
{
    public class JobDescriptionController : Controller
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

        public IActionResult Create(JobdescriptionVM model)
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]

        public IActionResult Edit(JobdescriptionVM model)
        {
            return View();
        }


        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(JobdescriptionVM model)
        {
            return View();
        }
    }
}
