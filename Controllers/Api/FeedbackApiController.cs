using Marketplace.Dto;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers.Api
{
	[Route("api/feedback")]
	[ApiController]
	public class FeedbackApiController : ControllerBase
	{
		private readonly IFeedbackRepository feedbackRepository;

		public FeedbackApiController(IFeedbackRepository feedbackRepository)
		{
			this.feedbackRepository = feedbackRepository;
		}

		[HttpGet("{sellerId}")]
		public async Task<IActionResult> Get(int sellerId, int pageIndex, int pageSize)
		{
			try
			{
				if (pageSize < 1)
					pageSize = 1;
				else if (pageSize > 100)
					pageSize = 100;

				return Ok(new PageDto<FeedbackDto>(await feedbackRepository.GetFeedbackAsync(sellerId, pageIndex, pageSize)));
			}
			catch
			{
				return BadRequest();
			}
		}

		[Authorize]
		[HttpGet("left/{sellerId}")]
		public async Task<IActionResult> Get(int sellerId)
		{
			try
			{
				return Ok(await feedbackRepository.GetLeftFeedbackAsync(sellerId));
			}
			catch
			{
				return BadRequest();
			}
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
		[HttpDelete("{sellerId}")]
		public async Task<IActionResult> Delete(int sellerId)
		{
			try
			{
				await feedbackRepository.RemoveFeedbackAsync(sellerId);
				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
