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

    public class CategoriesController : Controller
    {

        private readonly JobPortalContext _db;

        public CategoriesController(JobPortalContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            CategoriesVM model = new CategoriesVM();
            model.CategoriesList = (from i in _db.Categories    //SElECT * FROM Categories
                                    where i.DeltetedDate == null
                                    select new CategoriesVM
                                    {
                                        CategoryId = i.CategoryId,   //converting from Entitymdoel to viewmodel
                                        CategoryName = i.CategoryName,
                                    }).ToList();
            return View(model);
        }

        //If empty , it will be a GET request

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Create(CategoriesVM model)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Categories entitycat = new Categories();
            entitycat.CategoryName = model.CategoryName;
            entitycat.CreatedBy = UserId; //Set the created by field
            entitycat.CreatedDate = DateTime.Now; //Set the created date field
            _db.Entry(entitycat).State = EntityState.Added; //Add the entity to the database
            _db.SaveChanges(); //Save the changes to the database
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var data = _db.Categories.FirstOrDefault(x => x.CategoryId == id);
            if (data == null)
            {
                return NotFound();
            }

            CategoriesVM model = new CategoriesVM
            {
                CategoryId = data.CategoryId,
                CategoryName = data.CategoryName
            };

            return View(model);
            
        }

        [HttpPost]

        public IActionResult Edit(CategoriesVM model)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Categories entitycat = new Categories();
            entitycat.CategoryId = model.CategoryId;
            entitycat.CategoryName = model.CategoryName;
            entitycat.UpdatedBy = UserId; //Set the created by field
            entitycat.UpdatedDate = DateTime.Now; //Set the created date field
            _db.Entry(entitycat).State = EntityState.Modified; //Add the entity to the database
            //_db.Entry(entityorg).Property(x => x.CreatedBy).IsModified = false;  // so that these values were removed when updating database
            //_db.Entry(entityorg).Property(x => x.CreatedDate).IsModified = false;
            _db.SaveChanges(); //Save the changes to the database
            return RedirectToAction("Index");
            
        }


        public IActionResult Delete(int id)
        {
            var data = _db.Categories.FirstOrDefault(x => x.CategoryId == id);
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

        [HttpPost]
        public IActionResult Delete(CategoriesVM model)
        {
            return View();
        }
    }
}
