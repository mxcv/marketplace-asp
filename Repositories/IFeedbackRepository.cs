using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.ViewModels;

namespace Marketplace.Repositories
{
	public interface IFeedbackRepository
	{
		Task<PaginatedList<FeedbackDto>> GetFeedbackAsync(int userId, int pageIndex, int pageSize);
		Task<int> AddFeedbackAsync(ApiFeedbackViewModel model);
		Task RemoveFeedbackAsync(int id);
	}
}
