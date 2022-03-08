using System.Security.Claims;
using Marketplace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		public static readonly string DirectoryPath = "files";

		private MarketplaceDbContext db;
		private IWebHostEnvironment appEnvironment;

		public ImagesController(MarketplaceDbContext db, IWebHostEnvironment appEnvironment)
		{
			this.db = db;
			this.appEnvironment = appEnvironment;
			Directory.CreateDirectory(Path.Combine(appEnvironment.WebRootPath, DirectoryPath));
		}

		[HttpPost("items/{itemId}")]
		public async Task<IActionResult> PostItemImages(int itemId, IFormFileCollection images)
		{
			foreach (var image in images)
			{
				Item? item = await db.Items.Where(x => x.Id == itemId).FirstOrDefaultAsync();
				int userId = GetUserId();
				if (item == null || item.UserId != userId)
					return BadRequest();

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
			return Ok();
		}

		[HttpPut("users")]
		public async Task<IActionResult> PutUserImage(IFormFile image)
		{
			int userId = GetUserId();
			UserImage? userImage = await db.UserImages
				.Where(x => x.UserId == userId)
				.Include(x => x.File)
				.FirstOrDefaultAsync();

			string extension = Path.GetExtension(image.FileName);
			if (userImage == null)
			{
				userImage = new UserImage() {
					UserId = userId,
					File = new MediaFile() {
						Extension = extension
					}
				};
				db.Add(userImage);
				await db.SaveChangesAsync();
			}
			else if (userImage.File.Extension != extension)
			{
				System.IO.File.Delete(GetFullPath(userImage.File.Name));
				userImage.File.Extension = extension;
				await db.SaveChangesAsync();
			}

			using (var stream = new FileStream(GetFullPath(userImage.File.Name), FileMode.Create))
				await image.CopyToAsync(stream);
			return Ok();
		}

		private int GetUserId()
		{
			return int.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());
		}

		private string GetFullPath(string filename)
		{
			return Path.Combine(appEnvironment.WebRootPath, DirectoryPath, filename);
		}
	}
}
