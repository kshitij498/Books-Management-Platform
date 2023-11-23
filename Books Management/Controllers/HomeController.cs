using Books_Management.Data;
using Books_Management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Books_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly ApplicationDbContext appDbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext appDbContext)
        {
            _logger = logger;
            this.appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            List<Product> product = appDbContext.Products.Include(u => u.Category).ToList();
            return View(product);
        }

        public IActionResult Details(int productId)
        {
            Product? product = appDbContext.Products
                                .Include(p => p.Category).FirstOrDefault(p => p.Id == productId);

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}