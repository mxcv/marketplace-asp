using System.Diagnostics;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	public class HomeController : Controller
	{
		private readonly IItemRepository itemRepository;

		public HomeController(IItemRepository itemRepository)
		{
			this.itemRepository = itemRepository;
		}

		public async Task<IActionResult> Index()
		{
			return View(await itemRepository.GetItemsAsync(new FilterViewModel(), null, 0, 20));
		}

		public IActionResult Privacy()
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
