using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class FeedbackController : ControllerBase
	{
		private readonly IFeedbackRepository feedbackRepository;

		public FeedbackController(IFeedbackRepository feedbackRepository)
		{
			this.feedbackRepository = feedbackRepository;
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Post(ApiFeedbackViewModel model)
		{
			try
			{
				return Ok(await feedbackRepository.AddFeedbackAsync(model));
			}
			catch
			{
				return BadRequest();
			}
		}

		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await feedbackRepository.RemoveFeedbackAsync(id);
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
