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

		[HttpGet]
		public async Task<IActionResult> Index(
			string? query,
			decimal? minPrice,
			decimal? maxPrice,
			int? category,
			int? currency,
			int? country,
			int? region,
			int? city,
			SortType? sort,
			int? page)
		{
			var model = await itemRepository.GetItems(new IndexViewModel(
				sort,
				new FilterViewModel() {
					Query = query,
					MinPrice = minPrice,
					MaxPrice = maxPrice,
					CurrencyId = currency,
					CategoryId = category,
					CountryId = country,
					RegionId = region,
					CityId = city,
				},
				new PageViewModel(page, null)
			));
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
