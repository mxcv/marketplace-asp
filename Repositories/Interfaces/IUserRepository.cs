using Marketplace.Dto;
using Marketplace.ViewModels;

namespace Marketplace.Repositories
{
	public interface IUserRepository
	{
		Task<UserDto> GetUser(int id);
		Task<UserDto> GetCurrentUser();
		Task<int> AddUser(ApiRegisterViewModel model);
		Task<int> AddUser(ApiRegisterViewModel model, IFormFile image);
	}
}
