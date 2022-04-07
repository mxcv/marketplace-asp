using Marketplace.ViewModels;

namespace Marketplace.Repositories
{
	public interface IItemRepository
	{
		Task<IndexViewModel> GetItems(IndexViewModel model);
		Task<IndexViewModel> GetMyItems(IndexViewModel model);
		Task<int?> AddItem(ApiItemViewModel model);
		Task<bool> RemoveItem(int id);
	}
}
