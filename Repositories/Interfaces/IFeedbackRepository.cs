using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.ViewModels;

namespace Marketplace.Repositories
{
	public interface IFeedbackRepository
	{
		Task<PaginatedList<FeedbackDto>> GetFeedbackAsync(int sellerId, int pageIndex, int pageSize);
		Task<FeedbackDto?> GetLeftFeedbackAsync(int sellerId);
		Task<int> AddFeedbackAsync(ApiFeedbackViewModel model);
		Task RemoveFeedbackAsync(int sellerId);
	}
}
