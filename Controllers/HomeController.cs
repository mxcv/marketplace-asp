using System.Diagnostics;
using Marketplace.Models;
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

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[Authorize]
		public async Task<IActionResult> MyItems()
		{
			return View((await itemRepository.GetMyItems(new ItemRequest())).Items);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
