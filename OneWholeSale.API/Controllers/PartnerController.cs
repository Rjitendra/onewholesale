using Microsoft.AspNetCore.Mvc;
using OneWholeSale.Model.Dto.Partner;
using Microsoft.AspNetCore.Authorization;
using OneWholeSale.Service.Interfaces;

namespace OneWholeSale.API.Controllers
{
    public class PartnerController : BaseController
    {
        public PartnerController(Ipartner service)
        {
            this.Service = service;
        }

        private Ipartner Service { get; }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Post([FromBody] PartnerDto dto)
        {
            return this.ProcessResult(await this.Service.AddPartner(dto));
        }

        [HttpPost("Update")]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] PartnerDto dto)
        {
            return this.ProcessResult(await this.Service.UpdatePartner(dto));
        }

        [HttpGet("{id:int}")]
        [Route("GetPartner/{id}")]
        public async Task<IActionResult> GetPartner(int id)
        {
            return this.ProcessResult(await this.Service.GetPartner(id));
        }

        [HttpDelete("{id:int}")]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return this.ProcessResult(await this.Service.DeletePartner(id));
        }
        [HttpGet]
        [Route("FullfimentCenter")]
        public async Task<IActionResult> FullfimentCenter()
        {
            var items = await Task.FromResult(await this.Service.GetFulfilmentCenter());
            return Ok(items);
        }

        [HttpGet]
        [Route("PartnerList")]
        public async Task<IActionResult> GetPartnerList()
        {
            var items = await Task.FromResult(await this.Service.GetpartnerList());
            return Ok(items);
        }
    }

   
}
