using Marketplace.Models;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	public class ItemsController : Controller
	{
		private IItemRepository itemRepository;

		public ItemsController(IItemRepository itemRepository)
		{
			this.itemRepository = itemRepository;
		}

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
			return View(await itemRepository.GetItems(new IndexViewModel(
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
			)));
		}

		[Authorize]
		public async Task<IActionResult> My()
		{
			return View((await itemRepository.GetMyItems(new IndexViewModel())).Items);
		}
	}
}
