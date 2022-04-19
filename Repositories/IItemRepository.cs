using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.ViewModels;

namespace Marketplace.Repositories
{
	public interface IItemRepository
	{
		Task<ItemDto> GetItem(int id);
		Task<IndexViewModel> GetItems(FilterViewModel filter, SortType? sortType, int pageIndex, int pageSize);
		Task<int> AddItem(ApiItemViewModel model);
		Task<int> AddItem(ApiItemViewModel model, IFormFileCollection images);
		Task RemoveItem(int id);
		Task RemoveItemRange(IEnumerable<int> id);
	}
}
