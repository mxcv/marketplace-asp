using Marketplace.Dto;

namespace Marketplace.Repositories
{
	public interface IUserRepository
	{
		Task<UserDto> GetUser(int id);
	}
}
