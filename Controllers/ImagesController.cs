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
		private MarketplaceContext db;
		private IWebHostEnvironment appEnvironment;
		public static readonly string DirectoryPath = "files";

		public ImagesController(MarketplaceContext db, IWebHostEnvironment appEnvironment)
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
				int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());
				if (item == null || item.UserId != userId)
					return BadRequest();

				ItemImage itemImage = new ItemImage() {
					Item = item,
					File = new MediaFile() {
						Extension = Path.GetExtension(image.FileName)
					}
				};
				db.Add(itemImage);
				await db.SaveChangesAsync();

				string path = Path.Combine(appEnvironment.WebRootPath, DirectoryPath, itemImage.File.Name);
				using (var stream = new FileStream(path, FileMode.Create))
					await image.CopyToAsync(stream);
			}
			return Ok();
		}
	}
}
