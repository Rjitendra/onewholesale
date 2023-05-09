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
		[Authorize(Policy = "AdminOnly")]
		public async Task<IActionResult> Dashboard()
        {
            var token = Request.Cookies["token"];
            if (token != null) {

                // make the API request with the token
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token+1);
                var response = await client.GetAsync("http://localhost:6001/api/Test");
                // handle the response
                if (response.IsSuccessStatusCode)
                {
                    // parse the response
                    var content = await response.Content.ReadAsStringAsync();
                  //  var result = JsonConvert.DeserializeObject<MyApiResponse>(content);

                    // return a success result
                   // return Json(new { success = true, data = result });
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    // handle unauthorized error (e.g. redirect to login)
                  //  return Json(new { success = false, error = "Unauthorized" });
                }
                else
                {
                    // handle other errors
                 //   return Json(new { success = false, error = "An error occurred" });
                }
            }
            return View();
        }
     
       

      
       
    }
}