using Marketplace.Dto;
using Marketplace.Exceptions;
using Marketplace.Models;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Marketplace.Controllers
{
	public class ItemsController : Controller
	{
		private readonly IItemRepository itemRepository;
		private readonly IStringLocalizer<ItemsController> localizer;

		public ItemsController(IItemRepository itemRepository, IStringLocalizer<ItemsController> localizer)
		{
			this.itemRepository = itemRepository;
			this.localizer = localizer;
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

		public async Task<IActionResult> Item(int id)
		{
			return View(await itemRepository.GetItem(id));
		}

		public new async Task<IActionResult> User(int id)
		{
			return View((await itemRepository.GetItems(new IndexViewModel(null, new FilterViewModel() { UserId = id }, null))).Items);
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> My()
		{
			return View((await itemRepository.GetMyItems(new IndexViewModel())).Items);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> My(int[] id)
		{
			await itemRepository.RemoveItemRange(id);
			return RedirectToAction("My");
		}

		[Authorize]
		[HttpGet]
		public IActionResult Add()
		{
			return View(new ItemViewModel());
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Add(ItemViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			model.Currency = new CurrencyDto() { Id = model.CurrencyId };
			if (model.CategoryId.HasValue)
				model.Category = new CategoryDto() { Id = model.CategoryId.Value };

			try
			{
				if (model.Images == null)
					await itemRepository.AddItem(model);
				else
					await itemRepository.AddItem(model, model.Images);
			}
			catch (FileCountOutOfBoundsException)
			{
				ModelState.AddModelError(string.Empty, localizer["ImageCountOutOfBounds"]);
				return View(model);
			}
			catch
			{
				ModelState.AddModelError(string.Empty, localizer["AddItemError"]);
				return View(model);
			}
			return RedirectToAction("My");
		}
	}
}
