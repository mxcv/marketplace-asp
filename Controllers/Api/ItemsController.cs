using System.Diagnostics;
using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public partial class ItemsController : ControllerBase
	{
		private IItemRepository itemRepository;

		public ItemsController(IItemRepository itemRepository)
		{
			this.itemRepository = itemRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Get(
			string? query,
			decimal? minPrice,
			decimal? maxPrice,
			int? currencyId,
			int? categoryId,
			int? countryId,
			int? regionId,
			int? cityId,
			int? userId,
			SortType? sortTypeId,
			int? pageIndex,
			int? pageSize)
		{
			var model = await itemRepository.GetItems(new IndexViewModel(
				sortTypeId,
				new FilterViewModel() {
					Query = query,
					MinPrice = minPrice,
					MaxPrice = maxPrice,
					CurrencyId = currencyId,
					CategoryId = categoryId,
					CountryId = countryId,
					RegionId = regionId,
					CityId = cityId,
					UserId = userId
				},
				new PageViewModel(pageIndex, pageSize)
			));

			Debug.Assert(model.Page != null);
			return model.Items == null ? BadRequest() : Ok(new PageDto(model.Items, model.Page.TotalItems));
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Post(ApiItemViewModel itemModel)
		{
			int? itemId = await itemRepository.AddItem(itemModel);
			return itemId == null ? BadRequest() : Ok(itemId);
		}

		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			return await itemRepository.RemoveItem(id) ? Ok() : BadRequest();
		}
	}
}
