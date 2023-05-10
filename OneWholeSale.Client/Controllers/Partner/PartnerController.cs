using Microsoft.AspNetCore.Mvc;

namespace OneWholeSale.Client.Controllers.Partner
{
	public class PartnerController : Controller
	{
		public IActionResult PurchaseOrder()
		{
			return View();
		}
		public IActionResult PurchaseOrderStatus()
		{
			return View();
		}
		public IActionResult DeliveryStatus()
		{
			return View();
		}
		public IActionResult StockAvailabel()
		{
			return View();
		}
	}
}
