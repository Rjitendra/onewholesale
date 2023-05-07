namespace OneWholeSale.Client.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OneWholeSale.Client.Models;
    using System.Diagnostics;
    using System.Net.Http.Headers;
    using OneWholeSale.Client.Extentions;
    using NuGet.Common;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json.Linq;
    using System.IdentityModel.Tokens.Jwt;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider _serviceProvider;
        public HomeController(IServiceProvider serviceProvider, ILogger<HomeController> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
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
            string accessToken = null;


            if (User.Identity.IsAuthenticated)
            {
                var accessTokenClaim = User.FindFirst("access_token");
                if (accessTokenClaim != null)
                {
                    accessToken = accessTokenClaim.Value;
                    // use the access token in your API requests
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(accessToken);
                    var expiration = token.ValidTo;
                    var currentUtcTime = DateTime.UtcNow;
                }
                else
                {
                    // access token not found
                }
            }
            // retrieve the access token from the ClaimsPrincipal

            if (accessToken != null)
            {
                // Get the UnauthorizedRequestHandler instance from the service provider
                var handler = _serviceProvider.GetService<UnauthorizedRequestHandler>();
                // Set the inner handler for the UnauthorizedRequestHandler instance
                handler.InnerHandler = new HttpClientHandler();

                // Create the HttpClient instance with the handler
                var client = new HttpClient(handler);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync("http://localhost:6001/api/Test");
                if (response.IsSuccessStatusCode)
                {
                    // Handle successful response
                    return View();
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