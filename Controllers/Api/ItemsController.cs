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
			int? categoryId,
			int? countryId,
			int? regionId,
			int? cityId,
			int? userId,
			SortType? sortTypeId,
			int? skipCount,
			int? takeCount)
		{
			var model = await itemRepository.GetItems(new IndexViewModel() {
				Filter = {
					Query = query,
					MinPrice = minPrice,
					MaxPrice = maxPrice,
					CategoryId = categoryId,
					CountryId = countryId,
					RegionId = regionId,
					CityId = cityId,
					UserId = userId,
					//SkipCount = skipCount,
					//TakeCount = takeCount
				},
				SortType = sortTypeId,
			});
			return model.Items == null ? BadRequest() : Ok(new PageDto(model.Items, 0));
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
