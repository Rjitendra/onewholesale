using Microsoft.AspNetCore.Mvc;
using OneWholeSale.Model.Dto;
using OneWholeSale.Model.Dto.Partner;
using OneWholeSale.Service.Implementations.Interfaces;

namespace OneWholeSale.API.Controllers
{
    public class KiranaController : BaseController
    {
        private IKirana Service { get; }
        public KiranaController(IKirana service)
        {
            this.Service = service;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Post([FromBody] KiranaDto dto)
        {
            return this.ProcessResult(await this.Service.AddKirana(dto));
        }

        [HttpPost("Update")]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] KiranaDto dto)
        {
            return this.ProcessResult(await this.Service.UpdateKirana(dto));
        }

        [HttpGet("{id:int}")]
        [Route("GetKirana/{id}")]
        public async Task<IActionResult> GetKirana(int id)
        {
            return this.ProcessResult(await this.Service.GetKirana(id));
        }

        [HttpDelete("{id:int}")]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return this.ProcessResult(await this.Service.DeleteKirana(id));
        }

        [HttpGet]
        [Route("Partner")]
        public async Task<IActionResult> Partner()
        {
            var items = await Task.FromResult(await this.Service.GetpartnerList());
            return Ok(items);
        }
        [HttpGet]
        [Route("KiranaList")]
        public async Task<IActionResult> GetKiranaDetails()
        {
            var items = await Task.FromResult(await this.Service.GetKiranaDetailsList());
            return Ok(items);
        }




    }
}
