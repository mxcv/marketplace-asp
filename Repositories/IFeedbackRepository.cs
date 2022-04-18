using Marketplace.ViewModels;

namespace Marketplace.Repositories
{
	public interface IFeedbackRepository
	{
		Task<int> AddFeedbackAsync(ApiFeedbackViewModel model);
		Task RemoveFeedbackAsync(int id);
	}
}
