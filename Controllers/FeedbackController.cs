using Marketplace.Repositories;
using Marketplace.ViewModels;
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
				return View(new UserFeedbackViewModel(
					await userRepository.GetUser(id),
					await feedbackRepository.GetFeedbackAsync(id, page, 20)
				));
			}
			catch
			{
				return NotFound();
			}
		}
	}
}
