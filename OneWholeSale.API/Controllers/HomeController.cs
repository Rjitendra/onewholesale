namespace OneWholeSale.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("/docs")]
        [Route("/swagger")]
        public IActionResult Index()
        {
            if (HttpContext.Request.Path.StartsWithSegments("/swagger"))
            {
                return View(); // or any other action you want to perform if already on /swagger route
            }
            else
            {
                return new RedirectResult("~/swagger");
            }
        }
    }
}