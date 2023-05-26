using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneWholeSale.Model.Dto;
using OneWholeSale.Model.Dto.Partner;
using OneWholeSale.Model.Dto.SalesPerson;
using OneWholeSale.Model.Entity.Master;
using OneWholeSale.Model.Entity.Partner;
using OneWholeSale.Model.Entity.SalesPerson;
using OneWholeSale.Model.Utility;
using System.Net.Http.Headers;
using System.Text;

namespace OneWholeSale.Client.Controllers.SalesPartner
{
	public class SalesPartnerController : Controller
	{
        private readonly IHttpContextAccessor _httpContextAccessor;
       
        public SalesPartnerController(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
		{
			return View();
		}
        [HttpGet]
        public async Task<IActionResult> Partner(int? id)
        {
            var roleName = _httpContextAccessor.HttpContext.User.FindFirst(ApplicationClaims.RoleName)?.Value;
            if (roleName.ToString() == "Admin")
            {
                var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
                var tokenvalue = tokenClaim?.Value;
                var httpClient = new HttpClient();

                var tokenEndpoint = "http://localhost:6001/api/Partner/FullfimentCenter";
                var tokenEndpoint2 = "http://localhost:6001/api/Partner/PartnerList";
                // Add the JWT token to the request headers
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenvalue);
                var tokenResponse = await httpClient.GetAsync(tokenEndpoint);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

                var tokenResponse2 = await httpClient.GetAsync(tokenEndpoint2);
                var tokenContent2 = await tokenResponse2.Content.ReadAsStringAsync();

                if (tokenResponse.IsSuccessStatusCode && tokenResponse2.IsSuccessStatusCode)
                {
                    var tokenObject = JObject.Parse(tokenContent);
                    var fulfillmet = tokenObject["entity"];
                    List<SelectListItem> li1 = new List<SelectListItem>();
                    li1.Add(new SelectListItem { Text = "--Select Occupation--", Value = "0" });

                    foreach (var item in fulfillmet)
                    {
                        var fulfilmentcenter = (string)item["name"];
                        var fulfillmentid = (int)item["id"];
                        li1.Add(new SelectListItem
                        {
                            Text = fulfilmentcenter,
                            Value = fulfillmentid.ToString()
                        });
                    }
                    ViewBag.Fulfillment = li1;

                    var tokenObject2 = JObject.Parse(tokenContent2);
                    var entityArray = tokenObject2["entity"];

                    var Partnerlist = entityArray.ToObject<List<PartnerDto>>();
                    ViewBag.PartnerList = Partnerlist;
                }
                if (id == 0 || id == null)
                {

                    return View();
                }
                else
                {
                    var result = await Get(id);
                    PartnerDto part = null;
                    if (result is JsonResult jsonResult)
                    {
                        part = jsonResult.Value as PartnerDto;
                    }
                    return View(part);
                   
                }
            }

            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Partner(PartnerDto dto)
        {
            if (dto.Id == null || dto.Id == 0)
            {
                dto.PartnerCode = "";

                var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
                var tokenValue = tokenClaim?.Value;

                var httpClient = new HttpClient();
                var tokenEndpoint = "http://localhost:6001/api/Partner/Create";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenValue);

                var requestData = dto;
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var tokenResponse = await httpClient.PostAsync(tokenEndpoint, requestContent);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                if (tokenResponse.IsSuccessStatusCode)
                {
                    TempData["Msg"] = "Partner Created Sucessfully";
                    return RedirectToAction("Partner");
                }
                else
                {
                    TempData["WMsg"] = "Something Went Wrong";
                    return RedirectToAction("Partner");
                }
            }
            else
            {
                var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
                var tokenValue = tokenClaim?.Value;

                var httpClient = new HttpClient();
                var tokenEndpoint = "http://localhost:6001/api/Partner/Update";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenValue);

                var requestData = dto;
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var tokenResponse = await httpClient.PostAsync(tokenEndpoint, requestContent);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                if (tokenResponse.IsSuccessStatusCode)
                {
                    TempData["Msg"] = "Partner Updated Sucessfully";
                    return RedirectToAction("Partner");
                }
                else
                {
                    TempData["WMsg"] = "Something Went Wrong";
                    return RedirectToAction("Partner");
                }
            }

        }

        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {

            var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
            var tokenvalue = tokenClaim?.Value;
            var httpClient = new HttpClient();
            var baseEndpoint = "http://localhost:6001";
            var tokenEndpoint = $"{baseEndpoint}/api/Partner/GetPartner/{id}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenvalue);
            var tokenResponse = await httpClient.GetAsync(tokenEndpoint);
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

            if (tokenResponse.IsSuccessStatusCode)
            {
                var partner = JsonConvert.DeserializeObject<PartnerDto>(tokenContent);
                return Json(partner);
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
            var tokenEndpoint = $"{baseEndpoint}/api/Partner/Delete/{id}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenvalue);
            var tokenResponse = await httpClient.GetAsync(tokenEndpoint);
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

            if (tokenResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("Partner");

            }
            else
            {
                return BadRequest();
            }
        }




        [HttpGet]
        public async Task<IActionResult> KiranaMaster(int? id)
        {
            var roleName = _httpContextAccessor.HttpContext.User.FindFirst(ApplicationClaims.RoleName)?.Value;
            if (roleName.ToString() == "Admin")
            {
                var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
                var tokenvalue = tokenClaim?.Value;
                var httpClient = new HttpClient();
                var tokenEndpoint = "http://localhost:6001/api/Kirana/KiranaList";
                var tokenEndpointforpartner = "http://localhost:6001/api/Kirana/Partner";

                // Add the JWT token to the request headers
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenvalue);
                var tokenResponse = await httpClient.GetAsync(tokenEndpoint);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

                var tokenResponse2 = await httpClient.GetAsync(tokenEndpointforpartner);
                var tokenContent2 = await tokenResponse2.Content.ReadAsStringAsync();

                if (tokenResponse.IsSuccessStatusCode && tokenResponse2.IsSuccessStatusCode)
                {
                    var tokenObject = JObject.Parse(tokenContent);
                    var entityArray = tokenObject["entity"];

                    var tokenObject2 = JObject.Parse(tokenContent2);
                    var partner = tokenObject2["entity"];

                    List<SelectListItem> li1 = new List<SelectListItem>();
                    li1.Add(new SelectListItem { Text = "--Select Occupation--", Value = "0" });

                    foreach (var item in partner)
                    {
                        var parter = (string)item["name"];
                        var partnerid = (int)item["id"];
                        li1.Add(new SelectListItem
                        {
                            Text = parter,
                            Value = partnerid.ToString()
                        });
                    }
                    ViewBag.partner = li1;

                    var KiranaList = entityArray.ToObject<List<KiranaDto>>();
                    ViewBag.KiranaList = KiranaList;

                }
                if (id == 0 || id == null)
                {

                    return View();
                }
                else
                {
                    var result = await GetKirana(id);
                    KiranaDto kirana = null;
                    if (result is JsonResult jsonResult)
                    {
                        kirana = jsonResult.Value as KiranaDto;
                    }
                    return View(kirana);
                    
                }
            }

            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        [HttpPost]
        public async Task<IActionResult> KiranaMaster(KiranaDto dto)
        {
            if (dto.Id == null || dto.Id == 0)
            {
                dto.KiranaCode = "";
                dto.ContactPerson = "";
                dto.AddBy = "0";
                var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
                var tokenValue = tokenClaim?.Value;

                var httpClient = new HttpClient();
                var tokenEndpoint = "http://localhost:6001/api/Kirana/Create";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenValue);

                var requestData = dto;
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var tokenResponse = await httpClient.PostAsync(tokenEndpoint, requestContent);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                if (tokenResponse.IsSuccessStatusCode)
                {
                    TempData["Msg"] = "Kirana Created Sucessfully";
                    return RedirectToAction("KiranaMaster");
                }
                else
                {
                    TempData["WMsg"] = "Something Went Wrong";
                    return RedirectToAction("KiranaMaster");
                }
            }
            else
            {
                var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
                var tokenValue = tokenClaim?.Value;

                var httpClient = new HttpClient();
                var tokenEndpoint = "http://localhost:6001/api/Kirana/Update";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenValue);

                var requestData = dto;
                var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                var tokenResponse = await httpClient.PostAsync(tokenEndpoint, requestContent);
                var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
                if (tokenResponse.IsSuccessStatusCode)
                {
                    TempData["Msg"] = "Kirana Updated Sucessfully";
                    return RedirectToAction("KiranaMaster");
                }
                else
                {
                    TempData["WMsg"] = "Something Went Wrong";
                    return RedirectToAction("KiranaMaster");
                }
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetKirana(int? id)
        {
            var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
            var tokenvalue = tokenClaim?.Value;
            var httpClient = new HttpClient();
            var baseEndpoint = "http://localhost:6001";
            var tokenEndpoint = $"{baseEndpoint}/api/Kirana/GetKirana/{id}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenvalue);
            var tokenResponse = await httpClient.GetAsync(tokenEndpoint);
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();
            if (tokenResponse.IsSuccessStatusCode)
            {
                var kirana = JsonConvert.DeserializeObject<KiranaDto>(tokenContent);
                return Json(kirana);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteKirana(int? id)
        {

            var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "access_token");
            var tokenvalue = tokenClaim?.Value;
            var httpClient = new HttpClient();
            var baseEndpoint = "http://localhost:6001";
            var tokenEndpoint = $"{baseEndpoint}/api/Kirana/Delete/{id}";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenvalue);
            var tokenResponse = await httpClient.GetAsync(tokenEndpoint);
            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

            if (tokenResponse.IsSuccessStatusCode)
            {
                return RedirectToAction("KiranaMaster");

            }
            else
            {
                return BadRequest();
            }
        }



    }
}
