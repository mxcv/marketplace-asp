namespace Marketplace.Repositories
{
	public interface IImageRepository
	{
		Task<bool> AddItemImagesAsync(int itemId, IFormFileCollection images);
		Task<bool> SetUserImageAsync(IFormFile image);
		Task<bool> RemoveItemImagesAsync(int itemId);
		string GetRelativeWebPath(string filename);
	}
}
