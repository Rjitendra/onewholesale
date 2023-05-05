using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneWholeSale.Client.Models;
using System.Diagnostics;

namespace OneWholeSale.Client.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //jjj
        public IActionResult Index()
        {
            return View();
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
        [Authorize]
        public IActionResult Dashboard()
        {
            bool s = HttpContext.User.Identity.IsAuthenticated;
            return View();
        }
        public IActionResult Country()
        {
            return View();
        }

        public IActionResult State()
        {
            return View();
        }
        public IActionResult District()
        {
            return View();
        }
        public IActionResult SalesPersonDetails()
        {
            return View();
        }

        public IActionResult FulfilmentCenter()
        {
            return View();
        }

        public IActionResult Partner()
        {
            return View();
        }
        public IActionResult KiranaMaster()
        {
            return View();
        }
        public IActionResult VendorMaster()
        {
            return View();
        }
        public IActionResult VendorContactPerson()
        {
            return View();
        }
        public IActionResult VendorCategory()
        {
            return View();
        }
        public IActionResult VendorSubCategory()
        {
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
    }
}