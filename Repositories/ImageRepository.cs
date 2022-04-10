using System.Security.Claims;
using System.Security.Principal;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
	public class ImageRepository : IImageRepository
	{
		private static readonly string DirectoryPath = "files";
		private static readonly int MaxItemImages = 7;

		private MarketplaceDbContext db;
		private IWebHostEnvironment appEnvironment;
		private int? userId;

		public ImageRepository(MarketplaceDbContext db, IWebHostEnvironment appEnvironment, IPrincipal principal)
		{
			this.db = db;
			this.appEnvironment = appEnvironment;
			Directory.CreateDirectory(Path.Combine(appEnvironment.WebRootPath, DirectoryPath));

			string? identifier = ((ClaimsPrincipal)principal).FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (identifier != null)
				userId = int.Parse(identifier);
		}

		public async Task<bool> AddItemImagesAsync(int itemId, IFormFileCollection images)
		{
			if (await db.ItemImages.Where(x => x.ItemId == itemId).CountAsync() + images.Count > MaxItemImages)
				return false;

			foreach (var image in images)
			{
				Item? item = await db.Items.Where(x => x.Id == itemId).FirstOrDefaultAsync();
				if (item == null || item.UserId != userId)
					return false;

				ItemImage itemImage = new ItemImage() {
					ItemId = itemId,
					File = new MediaFile() {
						Extension = Path.GetExtension(image.FileName)
					}
				};
				db.Add(itemImage);
				await db.SaveChangesAsync();

				using (var stream = new FileStream(GetFullPath(itemImage.File.Name), FileMode.Create))
					await image.CopyToAsync(stream);
			}
			return true;
		}

		public async Task<bool> SetUserImageAsync(IFormFile image)
		{
			if (userId == null)
				return false;
			UserImage? userImage = await db.UserImages
				.Where(x => x.UserId == userId)
				.Include(x => x.File)
				.FirstOrDefaultAsync();

			string extension = Path.GetExtension(image.FileName);
			if (userImage == null)
			{
				userImage = new UserImage() {
					UserId = userId.Value,
					File = new MediaFile() {
						Extension = extension
					}
				};
				db.Add(userImage);
				await db.SaveChangesAsync();
			}
			else if (userImage.File.Extension != extension)
			{
				File.Delete(GetFullPath(userImage.File.Name));
				userImage.File.Extension = extension;
				await db.SaveChangesAsync();
			}

			using (var stream = new FileStream(GetFullPath(userImage.File.Name), FileMode.Create))
				await image.CopyToAsync(stream);
			return true;
		}

		public async Task<bool> RemoveItemImagesAsync(int itemId)
		{
			Item? item = await db.Items
				.Include(x => x.Images)
					.ThenInclude(x => x.File)
				.Where(x => x.Id == itemId)
				.FirstOrDefaultAsync();
			if (item == null || item.UserId != userId)
				return false;

			foreach (ItemImage image in item.Images)
				File.Delete(GetFullPath(image.File.Name));
			db.ItemImages.RemoveRange(item.Images);
			await db.SaveChangesAsync();
			return true;
		}

		public string GetRelativeWebPath(string filename)
		{
			return string.Format("/{0}/{1}", DirectoryPath, filename);
		}

		private string GetFullPath(string filename)
		{
			return Path.Combine(appEnvironment.WebRootPath, DirectoryPath, filename);
		}
	}
}
