using Marketplace.Dto;
using Marketplace.Models;

namespace Marketplace.ViewModels
{
	public class UserItemsViewModel
	{
		public UserItemsViewModel(UserDto user, PaginatedList<ItemDto> items)
		{
			User = user;
			Items = items;

			foreach (var item in items)
				item.User = user;
		}

		public UserDto User { get; set; }
		public PaginatedList<ItemDto> Items { get; set; }
	}
}
