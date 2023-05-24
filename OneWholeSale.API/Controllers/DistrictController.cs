
namespace OneWholeSale.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OneWholeSale.Service.Interfaces;
    [Authorize]
    public class DistrictController : BaseController
    {

        public DistrictController(ISalesPersonService service)
        {
            this.Service = service;
        }
        private ISalesPersonService Service { get; }

        [HttpGet]
        public async Task<IActionResult> Get_District()
        {
            var items = await Task.FromResult(await this.Service.GetDistrict_List());
            return Ok(items);
        }
    }
}
