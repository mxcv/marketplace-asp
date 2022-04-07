using System.Diagnostics;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	public class HomeController : Controller
	{
		private IItemRepository itemRepository;

		public HomeController(IItemRepository itemRepository)
		{
			this.itemRepository = itemRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return await Index(new IndexViewModel());
		}

		[HttpPost]
		public async Task<IActionResult> Index(IndexViewModel model)
		{
			return View(await itemRepository.GetItems(model));
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[Authorize]
		public async Task<IActionResult> MyItems()
		{
			return View((await itemRepository.GetMyItems(new IndexViewModel())).Items);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
