using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneWholeSale.Model.Dto.SalesPerson;
using OneWholeSale.Model.Entity.Master;
using OneWholeSale.Model.Entity.SalesPerson;
using OneWholeSale.Model.Utility;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OneWholeSale.Client.Controllers
{
	public class AdminController : Controller
	{
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminController(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
		{
			return View();
		}
        [HttpGet]
        public async Task<IActionResult> SalesPersonDetails(int? id)
		{
            var roleName = _httpContextAccessor.HttpContext.User.FindFirst(ApplicationClaims.RoleName)?.Value;
            if (roleName.ToString() == "Admin")
            {

              
                List<District> district = new List<District>();
                var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
                var tokenvalue = tokenClaim?.Value;
                var httpClient = new HttpClient();
                var tokenEndpoint = "http://localhost:6001/api/SalesPerson";
                var districtapi = "http://localhost:6001/api/District";
                // Add the JWT token to the request headers
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenvalue);
                var tokenResponse = await httpClient.GetAsync(tokenEndpoint);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                var districtresponse = await httpClient.GetAsync(districtapi);
                var districtContent = await districtresponse.Content.ReadAsStringAsync();
                List<SelectListItem> li = new List<SelectListItem>();

                li.Add(new SelectListItem
                {
                    Text = "--Select Country--",
                    Value = "0",
                });
                li.Add(new SelectListItem
                {
                    Text = "India",
                    Value = "India",
                });

                ViewBag.Country = li;

                List<SelectListItem> li11 = new List<SelectListItem>();

                li11.Add(new SelectListItem
                {
                    Text = "--Select State--",
                    Value = "0",
                });
                li11.Add(new SelectListItem
                {
                    Text = "Odisha",
                    Value = "Odisha",
                });
                ViewBag.state = li11;
                if (tokenResponse.IsSuccessStatusCode && districtresponse.IsSuccessStatusCode) 
                {
                    var tokenObject = JObject.Parse(tokenContent);
                    var entityArray = tokenObject["entity"];

                    var districtObject = JObject.Parse(districtContent);
                    var districtArray = districtObject["entity"];

                    List<SelectListItem> li1 = new List<SelectListItem>();
                    li1.Add(new SelectListItem { Text = "--Select District--", Value = "0" });

                    foreach (var item in districtArray)
                    {
                        var districtName = (string)item["districtName"];
                        var districtid = (int)item["id"];
                        li1.Add(new SelectListItem
                        {
                            Text = districtName,
                            Value = districtid.ToString()
                        });
                    }
                    ViewBag.District = li1;
                    var salesPersonList = entityArray.ToObject<List<Vw_SalesPerson>>();
                    ViewBag.SalesPerson = salesPersonList;
                }
                if(id == 0|| id == null)
                {
                    return View();
                }
                else  
                {
                    var result = await Get(id);
                    SalesPersonDto salesPerson = null;
                    if (result is JsonResult jsonResult)
                    {
                        salesPerson = jsonResult.Value as SalesPersonDto;
                    }
                    return View(salesPerson);
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {

            var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
            var tokenvalue = tokenClaim?.Value;
            var httpClient = new HttpClient();
            var baseEndpoint = "http://localhost:6001";
            var tokenEndpoint = $"{baseEndpoint}/api/SalesPerson/GetSales/{id}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenvalue);
            var tokenResponse = await httpClient.GetAsync(tokenEndpoint);
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

            if (tokenResponse.IsSuccessStatusCode)
            {
                var salesperson = JsonConvert.DeserializeObject<SalesPersonDto>(tokenContent);
                return Json(salesperson);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {

            var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
            var tokenvalue = tokenClaim?.Value;
            var httpClient = new HttpClient();
            var baseEndpoint = "http://localhost:6001";
            var tokenEndpoint = $"{baseEndpoint}/api/SalesPerson/Delete/{id}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenvalue);
            var tokenResponse = await httpClient.GetAsync(tokenEndpoint);
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

            if (tokenResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("SalesPersonDetails");
                    
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost]
        public async Task<IActionResult> SalesPersonDetails(SalesPersonDto dto)
        {
            if (dto .Id== null || dto.Id == 0)
            {
                dto.SalesPersonCode = "";
          
                var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
                var tokenValue = tokenClaim?.Value;

                var httpClient = new HttpClient();
                var tokenEndpoint = "http://localhost:6001/api/SalesPerson";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenValue);

                var requestData = dto;
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var tokenResponse = await httpClient.PostAsync(tokenEndpoint, requestContent);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                if (tokenResponse.IsSuccessStatusCode)
                {
                    TempData["Msg"] = "Sales Person Created Sucessfully";
                    return RedirectToAction("SalesPersonDetails");
                }
                else
                {
                    TempData["WMsg"] = "Something Went Wrong";
                    return RedirectToAction("SalesPersonDetails");
                }
            }
            else
            {
               
                var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
                var tokenValue = tokenClaim?.Value;

                var httpClient = new HttpClient();
                var tokenEndpoint = "http://localhost:6001/api/SalesPerson/Update";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenValue);

                var requestData = dto;
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var tokenResponse = await httpClient.PostAsync(tokenEndpoint, requestContent);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                if (tokenResponse.IsSuccessStatusCode)
                {
                    TempData["Msg"] = "Sales Person Updated Sucessfully";
                    return RedirectToAction("SalesPersonDetails");
                }
                else
                {
                    TempData["WMsg"] = "Something Went Wrong";
                    return RedirectToAction("SalesPersonDetails");
                }
            }
            
        }










        public IActionResult FulfilmentCenter()
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
