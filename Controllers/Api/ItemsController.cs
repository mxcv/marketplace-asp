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
			int? categoryId,
			int? countryId,
			int? regionId,
			int? cityId,
			int? userId,
			int? sortTypeId,
			int? skipCount,
			int? takeCount)
		{
			return Ok(await itemRepository.GetItems(new ItemRequest() {
				Query = query,
				MinPrice = minPrice,
				MaxPrice = maxPrice,
				CategoryId = categoryId,
				CountryId = countryId,
				RegionId = regionId,
				CityId = cityId,
				UserId = userId,
				SortTypeId = sortTypeId,
				SkipCount = skipCount,
				TakeCount = takeCount
			}));
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Post(ItemViewModel itemModel)
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
