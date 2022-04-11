namespace Marketplace.Repositories
{
	public interface IImageRepository
	{
		Task AddItemImagesAsync(int itemId, IFormFileCollection images);
		Task SetUserImageAsync(IFormFile image);
		Task RemoveItemImagesAsync(int itemId);
		string GetRelativeWebPath(string filename);
	}
}
