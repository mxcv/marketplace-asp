using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.ViewModels;

namespace Marketplace.Repositories
{
	public interface IItemRepository
	{
		Task<PageDto> GetItems(ItemRequest request);
		Task<PageDto> GetMyItems(ItemRequest request);
		Task<int?> AddItem(ApiItemViewModel model);
		Task<bool> RemoveItem(int id);
	}
}
