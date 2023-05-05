using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OneWholeSale.Model.Dto.Login;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;

namespace OneWholeSale.Client.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }



        [HttpGet]
        public async Task<IActionResult> Login()

        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(loginDto model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Home");
            }

            if (ModelState.IsValid)
            {
                model.RememberMe = true;
                var httpClient = new HttpClient();
                var tokenEndpoint = "http://localhost:6001/api/LogIn";

                var requestData = new { username = model.Username, password = model.Password };
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var tokenResponse = await httpClient.PostAsync(tokenEndpoint, requestContent);


                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

                if (tokenResponse.IsSuccessStatusCode)
                {
                    var tokenObject = JObject.Parse(tokenContent);
                    var token1 = (string)tokenObject["token"];
                    var expiration = (DateTime)tokenObject["expiration"];
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.ReadJwtToken(token1);

                    var claims = new List<Claim>();
                    claims.AddRange(token.Claims);

                    var claimsIdentity = new ClaimsIdentity(
                           claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(claimsIdentity);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(
                     CookieAuthenticationDefaults.AuthenticationScheme,
                     principal,
                     authProperties);

                    _logger.LogInformation("User {Email} logged in at {Time}.",
                        "jitendrabehera64@gmail.com", DateTime.UtcNow);
                    bool s = HttpContext.User.Identity.IsAuthenticated;
                    return RedirectToAction("Dashboard", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

            return View(model);
        }
    }
}