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
		private readonly IUserRepository userRepository;
		private readonly IStringLocalizer<ItemsController> localizer;

		public ItemsController(IItemRepository itemRepository, IUserRepository userRepository, IStringLocalizer<ItemsController> localizer)
		{
			this.itemRepository = itemRepository;
			this.userRepository = userRepository;
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

			return View(await itemRepository.GetItemsAsync(filter, sort, page, 20));
		}

		public async Task<IActionResult> Item(int id)
		{
			try
			{
				return View(await itemRepository.GetItemAsync(id));
			}
			catch
			{
				return NotFound();
			}
		}

		public new async Task<IActionResult> User(int id)
		{
			try
			{
				return View(new UserItemsViewModel(
					await userRepository.GetUser(id),
					(await itemRepository.GetItemsAsync(new FilterViewModel() { UserId = id }, null, 0, 0)).Items
				));
			}
			catch
			{
				return NotFound();
			}
		}

		[Authorize(Roles = "Seller")]
		[HttpGet]
		public async Task<IActionResult> My()
		{
			int id = int.Parse(base.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());
			return View(new UserItemsViewModel(
				await userRepository.GetUser(id),
				(await itemRepository.GetItemsAsync(new FilterViewModel() { UserId = id }, null, 0, 0)).Items
			));
		}

		[Authorize(Roles = "Seller")]
		[HttpPost]
		public async Task<IActionResult> My(int[] id)
		{
			await itemRepository.RemoveItemRangeAsync(id);
			return RedirectToAction("My");
		}

		[Authorize(Roles = "Seller")]
		[HttpGet]
		public IActionResult Add()
		{
			return View(new ItemViewModel());
		}

		[Authorize(Roles = "Seller")]
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
					await itemRepository.AddItemAsync(model);
				else
					await itemRepository.AddItemAsync(model, model.Images);
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

		[Authorize(Roles = "Moderator")]
		[HttpGet]
		public async Task<IActionResult> Remove(int id)
		{
			await itemRepository.RemoveItemAsync(id);
			return RedirectToAction("Index", "Home");
		}
	}
}
