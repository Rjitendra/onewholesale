using Microsoft.AspNetCore.Mvc;

namespace OneWholeSale.Client.Controllers.FulfillmentCenter
{
	public class FulfillmentController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult PurchaseOrder()
		{
			return View();
		}
		public IActionResult PurchaseOrderInvoice()
		{
			return View();
		}
		public IActionResult StockEntry()
		{
			return View();
		}

	}
}
