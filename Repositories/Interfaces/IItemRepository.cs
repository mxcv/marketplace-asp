using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.ViewModels;

namespace Marketplace.Repositories
{
	public interface IItemRepository
	{
		Task<ItemDto> GetItemAsync(int id);
		Task<IndexViewModel> GetItemsAsync(FilterViewModel filter, SortType? sortType, int pageIndex, int pageSize);
		Task<int> AddItemAsync(ApiItemViewModel model);
		Task<int> AddItemAsync(ApiItemViewModel model, IFormFileCollection images);
		Task RemoveItemAsync(int id);
		Task RemoveItemRangeAsync(IEnumerable<int> id);
	}
}
