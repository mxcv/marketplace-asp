using Marketplace.Dto;

namespace Marketplace.ViewModels
{
	public class ModeratorsViewModel
	{
		public ModeratorsViewModel()
		{
			Register = new ModeratorRegisterViewModel();
		}

		public ModeratorsViewModel(IEnumerable<UserDto> moderators) : this()
		{
			Moderators = moderators;
		}

		public ModeratorRegisterViewModel Register { get; set; }
		public IEnumerable<UserDto>? Moderators { get; set; }
	}
}
