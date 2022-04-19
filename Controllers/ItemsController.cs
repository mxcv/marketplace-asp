using System.Security.Claims;
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
			int page)
		{
			var filter = new FilterViewModel() {
				Query = query,
				MinPrice = minPrice,
				MaxPrice = maxPrice,
				CurrencyId = currency,
				CategoryId = category,
				CountryId = country,
				RegionId = region,
				CityId = city,
			};

			return View(await itemRepository.GetItems(filter, sort, page, 20));
		}

		public async Task<IActionResult> Item(int id)
		{
			return View(await itemRepository.GetItem(id));
		}

		public new async Task<IActionResult> User(int id)
		{
			return View(await GetUserItemsAsync(id));
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> My()
		{
			string userId = base.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
				?? throw new NullReferenceException();
			return View(await GetUserItemsAsync(int.Parse(userId)));
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

		public async Task<IEnumerable<ItemDto>> GetUserItemsAsync(int userId)
		{
			return (await itemRepository.GetItems(new FilterViewModel() { UserId = userId }, null, 0, 0)).Items;
		}
	}
}
