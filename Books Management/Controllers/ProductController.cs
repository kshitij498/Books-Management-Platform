using Books_Management.Data;
using Books_Management.Models;
using Books_Management.Models.ViewModels;
using Books_Management.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Books_Management.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        public readonly ApplicationDbContext appDbContext;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext appDbcontext, IWebHostEnvironment webHostEnvironment)
        {
            this.appDbContext = appDbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> product = appDbContext.Products.Include(u => u.Category).ToList();
            return View(product);
        }

        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = appDbContext.Categories
                .ToList().Select(u => new SelectListItem
            {
                Text=u.Name,
                Value=u.Id.ToString()
            });
            
            ProductVM productVM = new ()
            {
                Product = new Product(),
                categoryList = CategoryList
            };

            if(id==null || id==0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = appDbContext.Products.Find(id);
                return View(productVM);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\Product");
                    
                    if(!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                    var oldImageUrl = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                    if(System.IO.File.Exists(oldImageUrl))
                    {
                        System.IO.File.Delete(oldImageUrl);
                    }
                    }
                    
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.Product.ImageUrl = @"\images\Product\" + fileName;
                }

                if(obj.Product.Id == 0)
                {
                appDbContext.Products.Add(obj.Product);
                }
                else
                {
                appDbContext.Products.Update(obj.Product);
            }
            
            appDbContext.SaveChanges();
            TempData["success"] = "Product created successfully";
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = appDbContext.Products.Find(id);

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = appDbContext.Products.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            appDbContext.Products.Remove(obj);
            appDbContext.SaveChanges();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
