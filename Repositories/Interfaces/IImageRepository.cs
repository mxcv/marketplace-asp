namespace Marketplace.Repositories
{
	public interface IImageRepository
	{
		Task AddItemImagesAsync(int itemId, IFormFileCollection images);
		Task SetUserImageAsync(IFormFile image);
		Task SetUserImageAsync(IFormFile image, int userId);
		Task RemoveItemFileImagesAsync(int itemId);
	}
}
