using System.Diagnostics;
using Marketplace.Models;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private UserManager<User> userManager;

		public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
		{
			_logger = logger;
			this.userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Privacy()
		{
			await userManager.CreateAsync(
				new User() {
					UserName = "a",
					Email = "a",
					PhoneNumber = "555555",
					Name = "max",
					Created = DateTime.UtcNow,
					CityId = 1
				},
				"123456"
			);
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
