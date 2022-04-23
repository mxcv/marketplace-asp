using Marketplace.Dto;

namespace Marketplace.ViewModels
{
	public class UserViewModel
	{
		public UserViewModel(UserDto user, bool isLarge)
		{
			User = user;
			IsLarge = isLarge;
		}

		public UserDto User { get; set; }
		public bool IsLarge { get; set; }
	}
}
