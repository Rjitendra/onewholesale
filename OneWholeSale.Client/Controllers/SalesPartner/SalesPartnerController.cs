using Microsoft.AspNetCore.Mvc;

namespace OneWholeSale.Client.Controllers.SalesPartner
{
	public class SalesPartnerController : Controller
	{
		public IActionResult Index()
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
	}
}
