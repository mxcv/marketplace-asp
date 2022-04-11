﻿using Marketplace.Dto;
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

			if ((model.Images == null
				? await itemRepository.AddItem(model)
				: await itemRepository.AddItem(model, model.Images)) == null)
			{
				ModelState.AddModelError(string.Empty, "Could not add this item.");
				return View(model);
			}
			return RedirectToAction("My");
		}
	}
}
