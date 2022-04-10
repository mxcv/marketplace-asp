using Marketplace.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : ControllerBase
	{
		private IImageRepository imageRepository;

		public ImagesController(IImageRepository imageRepository)
		{
			this.imageRepository = imageRepository;
		}

		[HttpPost("items/{itemId}")]
		public async Task<IActionResult> PostItemImages(int itemId, IFormFileCollection images)
		{
			return await imageRepository.AddItemImagesAsync(itemId, images) ? Ok() : BadRequest();
		}

		[HttpPut("users")]
		public async Task<IActionResult> PutUserImage(IFormFile image)
		{
			return await imageRepository.SetUserImageAsync(image) ? Ok() : BadRequest();
		}
	}
}
