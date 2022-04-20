using Marketplace.Dto;
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

		[HttpGet]
		public async Task<IActionResult> Get(int userId, int pageIndex, int pageSize)
		{
			try
			{
				if (pageSize < 0)
					pageSize = 1;
				else if (pageSize > 100)
					pageSize = 100;

				return Ok(new PageDto<FeedbackDto>(await feedbackRepository.GetFeedbackAsync(userId, pageIndex, pageSize)));
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
