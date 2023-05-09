using Microsoft.AspNetCore.Mvc;

namespace OneWholeSale.Client.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult SalesPersonDetails()
		{
			return View();
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
