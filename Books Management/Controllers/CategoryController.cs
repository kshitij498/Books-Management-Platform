using Books_Management.Data;
using Books_Management.Models;
using Books_Management.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Books_Management.Controllers
{
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        public readonly ApplicationDbContext appDbContext;
        public CategoryController(ApplicationDbContext appDbcontext) 
        {
            this.appDbContext = appDbcontext;
        }
        public IActionResult Index()
        {
            List<Category> category = appDbContext.Categories.ToList();
            return View(category);
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            appDbContext.Categories.Add(category);
            appDbContext.SaveChanges();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = appDbContext.Categories.Find(id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                appDbContext.Categories.Update(obj);
                appDbContext.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = appDbContext.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = appDbContext.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            appDbContext.Categories.Remove(obj);
            appDbContext.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
