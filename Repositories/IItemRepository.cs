using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.ViewModels;

namespace Marketplace.Repositories
{
	public interface IItemRepository
	{
		Task<PageDto> GetItems(ItemRequest request);
		Task<int?> AddItem(ItemViewModel model);
		Task<bool> RemoveItem(int id);
	}
}
