using System.Diagnostics;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	public class HomeController : Controller
	{
		public HomeController()
		{
			
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
