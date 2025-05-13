using JobPortal.Data;
using JobPortal.Models.EntityModels;
using JobPortal.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobPortal.Controllers
{

    public class JobDescriptionController : Controller


    {
        private readonly JobPortalContext _db;

        public JobDescriptionController(JobPortalContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var model = new JobdescriptionVM();

            model.JobdescriptionList = (from i in _db.jobdescriptions
                                        where i != null // Adjust condition as needed
                                        select new JobdescriptionVM
                                        {
                                            JobId = i.JobId,
                                            Title = i.Title,
                                            Description = i.Description,
                                            CompanyName = i.CompanyName,
                                            Location = i.Location,
                                            JobType = i.JobType,
                                            JobPositions = i.JobPositions,
                                            MinSalary = i.MinSalary,
                                            MaxSalary = i.MaxSalary
                                        }).ToList();

            return View(model);
        }

        //If empty , it will be a GET request

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]


        public IActionResult Create(JobdescriptionVM model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var job = new Jobdescriptions
                {
                    Title = model.Title,
                    Description = model.Description,
                    CompanyName = model.CompanyName,
                    Location = model.Location,
                    JobType = model.JobType,
                    JobPositions = model.JobPositions,
                    MinSalary = model.MinSalary,
                    MaxSalary = model.MaxSalary,
                    // Optional if image path is relevant
                    // OrgImage = model.OrgImage
                };

                _db.jobdescriptions.Add(job);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
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
