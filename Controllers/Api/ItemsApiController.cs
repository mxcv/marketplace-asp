using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	[Route("api/items")]
	[ApiController]
	public partial class ItemsApiController : ControllerBase
	{
		private readonly IItemRepository itemRepository;

		public ItemsApiController(IItemRepository itemRepository)
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
			int pageIndex,
			int pageSize)
		{
			FilterViewModel filter = new FilterViewModel() {
				Query = query,
				MinPrice = minPrice,
				MaxPrice = maxPrice,
				CurrencyId = currencyId,
				CategoryId = categoryId,
				CountryId = countryId,
				RegionId = regionId,
				CityId = cityId,
				UserId = userId
			};

			if (userId == null)
			{
				if (pageSize < 1)
					pageSize = 1;
				else if (pageSize > 100)
					pageSize = 100;
			}

			var model = await itemRepository.GetItemsAsync(
				filter,
				sortTypeId,
				pageIndex,
				pageSize
			);

			return Ok(new PageDto<ItemDto>(model.Items));
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Post(ApiItemViewModel model)
		{
			try
			{
				return Ok(await itemRepository.AddItemAsync(model));
			}
			catch
			{
				return BadRequest();
			}
		}

		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await itemRepository.RemoveItemAsync(id);
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
