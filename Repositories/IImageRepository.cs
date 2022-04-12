namespace Marketplace.Repositories
{
	public interface IImageRepository
	{
		Task AddItemImagesAsync(int itemId, IFormFileCollection images);
		Task SetUserImageAsync(IFormFile image);
		Task RemoveItemFileImagesAsync(int itemId);
		string GetRelativeWebPath(string filename);
	}
}
