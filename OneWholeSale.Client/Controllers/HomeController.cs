using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OneWholeSale.Client.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace OneWholeSale.Client.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
      
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
        public async Task<IActionResult> Dashboard()
        {
            string accessToken=null;


            if (User.Identity.IsAuthenticated)
            {
                var accessTokenClaim = User.FindFirst("access_token");
                if (accessTokenClaim != null)
                {
                    accessToken = accessTokenClaim.Value;
                    // use the access token in your API requests
                }
                else
                {
                    // access token not found
                }
            }



            // retrieve the access token from the ClaimsPrincipal

            if (accessToken != null)
            {

                // make the API request with the token
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync("http://localhost:6001/api/Test");
                // handle the response
                if (response.IsSuccessStatusCode)
                {
                    
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("Login", "Account");
                }


            }
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