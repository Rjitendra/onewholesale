namespace OneWholeSale.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using OneWholeSale.Model.Dto.SalesPerson;
    using OneWholeSale.Service.Interfaces;

    [Authorize]
    public class SalesPersonController : BaseController
    {
        public SalesPersonController(ISalesPersonService service)
        {
            this.Service = service;
        }

        private ISalesPersonService Service { get; }

        /// <summary>
        ///Add Sales Person
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SalesPersonDto dto)
        {
            return this.ProcessResult(await this.Service.AddSalesPerson(dto));
        }

        /// <summary>
        ///Add Sales Person
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] SalesPersonDto dto)
        {
            return this.ProcessResult(await this.Service.UpdateSalesPerson(dto));
        }

        /// <summary>
        ///Get Sales Person by Id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return this.ProcessResult(await this.Service.GetSalesPerson(id));
        }

        /// <summary>
        ///Get Sales Person by Id
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return this.ProcessResult(await this.Service.DeleteSalesPerson(id));
        }


    }
}
