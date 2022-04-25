using Marketplace.Dto;
using Marketplace.Models;

namespace Marketplace.ViewModels
{
	public class UserFeedbackViewModel
	{
		public UserFeedbackViewModel(UserDto seller, PaginatedList<FeedbackDto> feedback)
		{
			Seller = seller;
			Feedback = feedback;
		}

		public UserDto Seller { get; set; }
		public PaginatedList<FeedbackDto> Feedback { get; set; }
	}
}
