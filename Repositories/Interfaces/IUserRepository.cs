using Marketplace.Dto;
using Marketplace.ViewModels;

namespace Marketplace.Repositories
{
	public interface IUserRepository
	{
		Task<UserDto> GetUser(int id);
		Task<UserDto> GetCurrentUser();
		Task<IEnumerable<UserDto>> GetModerators();
		Task<int> AddModerator(ModeratorRegisterViewModel model);
		Task RemoveModerator(int id);
		Task<int> AddSeller(ApiRegisterViewModel model);
		Task<int> AddSeller(ApiRegisterViewModel model, IFormFile image);
	}
}
