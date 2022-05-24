using System.Security.Claims;
using Marketplace.Dto;
using Marketplace.Repositories;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Controllers
{
	public class FeedbackController : Controller
	{
		private readonly IFeedbackRepository feedbackRepository;
		private readonly IUserRepository userRepository;

		public FeedbackController(IFeedbackRepository feedbackRepository, IUserRepository userRepository)
		{
			this.feedbackRepository = feedbackRepository;
			this.userRepository = userRepository;
		}

		public new async Task<IActionResult> User(int id, int page)
		{
			try
			{
				return View(await FillModelAsync(new UserFeedbackViewModel(), id, page));
			}
			catch
			{
				return NotFound();
			}
		}

		[Authorize]
		[HttpPost]
		public new async Task<IActionResult> User(UserFeedbackViewModel model)
		{
			try
			{
				if (!ModelState.IsValid)
					return View(await FillModelAsync(model, model.FeedbackViewModel.Seller.Id, 1));
				await feedbackRepository.AddFeedbackAsync(model.FeedbackViewModel);
				return RedirectToAction("User", new { id = model.FeedbackViewModel.Seller.Id });
			}
			catch
			{
				return BadRequest();
			}
		}

		[Authorize]
		public async Task<IActionResult> Remove(int id)
		{
			try
			{
				await feedbackRepository.RemoveFeedbackAsync(id);
				return RedirectToAction("User", new { id = id });
			}
			catch
			{
				return NotFound();
			}
		}

		public async Task<UserFeedbackViewModel> FillModelAsync(UserFeedbackViewModel model, int sellerId, int page)
		{
			string? userId = base.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			model.FeedbackViewModel = new FeedbackViewModel() {
				Seller = new UserDto() {
					Id = sellerId
				}
			};
			model.Seller = await userRepository.GetUser(sellerId);
			model.Feedback = await feedbackRepository.GetFeedbackAsync(sellerId, page, 20);
			model.LeftFeedback = await feedbackRepository.GetLeftFeedbackAsync(sellerId);
			model.CanLeaveFeedback = userId != null && userId != sellerId.ToString();
			return model;
		}
	}
}
