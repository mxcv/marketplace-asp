using Marketplace.Dto;
using Marketplace.Models;

namespace Marketplace.ViewModels
{
	public class UserFeedbackViewModel
	{
		public UserFeedbackViewModel()
		{
			FeedbackViewModel = new FeedbackViewModel();
			Seller = new UserDto();
			Feedback = new PaginatedList<FeedbackDto>();
		}

		public FeedbackViewModel FeedbackViewModel { get; set; }
		public UserDto Seller { get; set; }
		public PaginatedList<FeedbackDto> Feedback { get; set; }
		public FeedbackDto? LeftFeedback { get; set; }
		public bool CanLeaveFeedback { get; set; }
	}
}
