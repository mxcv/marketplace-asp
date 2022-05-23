using Marketplace.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	[Authorize(Roles = "Seller")]
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private readonly IImageRepository imageRepository;

		public ImagesController(IImageRepository imageRepository)
		{
			this.imageRepository = imageRepository;
		}

		[HttpPost("items/{itemId}")]
		public async Task<IActionResult> PostItemImages(int itemId, IFormFileCollection images)
		{
			try
			{
				await imageRepository.AddItemImagesAsync(itemId, images);
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}

		[HttpPut("users")]
		public async Task<IActionResult> PutUserImage(IFormFile image)
		{
			try
			{
				await imageRepository.SetUserImageAsync(image);
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
