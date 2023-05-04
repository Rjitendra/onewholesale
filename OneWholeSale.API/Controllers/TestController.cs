using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneWholeSale.Model.Dto.Login;

namespace OneWholeSale.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            return Ok();
        }
    }
}
