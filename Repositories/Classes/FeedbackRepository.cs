using System.Security.Claims;
using System.Security.Principal;
using Marketplace.Dto;
using Marketplace.Exceptions;
using Marketplace.Models;
using Marketplace.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
	public class FeedbackRepository : IFeedbackRepository
	{
		private readonly MarketplaceDbContext db;
		private readonly int? userId;

		public FeedbackRepository(MarketplaceDbContext db, IPrincipal principal)
		{
			this.db = db;

			string? identifier = ((ClaimsPrincipal)principal).FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (identifier != null)
				userId = int.Parse(identifier);
		}

		public async Task<PaginatedList<FeedbackDto>> GetFeedbackAsync(int sellerId, int pageIndex, int pageSize)
		{
			var feedback = db.Feedback
				.Include(x => x.Reviewer)
					.ThenInclude(x => x.ReceivedFeedback)
				.Include(x => x.Reviewer)
					.ThenInclude(x => x.Image)
						.ThenInclude(x => x!.File)
				.Where(x => x.SellerId == sellerId)
				.Select(x => new FeedbackDto() {
					Id = x.Id,
					Rate = x.Rate,
					Text = x.Text,
					Created = x.Created,
					Reviewer = new UserDto() {
						Id = x.Reviewer.Id,
						PhoneNumber = x.Reviewer.PhoneNumber,
						Name = x.Reviewer.Name,
						Created = x.Reviewer.Created,
						FeedbackStatistics = new FeedbackStatisticsDto(
							x.Reviewer.ReceivedFeedback.Count,
							x.Reviewer.ReceivedFeedback.Average(x => x.Rate)
						),
						City = x.Reviewer.CityId == null ? null : new CityDto() {
							Id = x.Reviewer.CityId.Value
						},
						Image = x.Reviewer.Image == null ? null : new ImageDto() {
							Path = ImageRepository.GetRelativeWebPath(x.Reviewer.Image.File.Name)
						}
					}
				});

			return await PaginatedList<FeedbackDto>.CreateAsync(feedback, pageIndex, pageSize);
		}

		public async Task<int> AddFeedbackAsync(ApiFeedbackViewModel model)
		{
			if (userId == null)
				throw new UnauthorizedUserException();
			if (userId == model.Seller.Id)
				throw new FeedbackException();

			Feedback feedback = new Feedback() {
				Rate = model.Rate,
				Text = model.Text,
				Created = DateTime.UtcNow,
				ReviewerId = userId.Value,
				SellerId = model.Seller.Id
			};

			db.Add(feedback);
			await db.SaveChangesAsync();
			return feedback.Id;
		}

		public async Task RemoveFeedbackAsync(int id)
		{
			if (userId == null)
				throw new UnauthorizedUserException();

			Feedback? feedback = await db.Feedback.Where(x => x.Id == id).FirstOrDefaultAsync();
			if (feedback == null)
				throw new NotFoundException();
			if (feedback.ReviewerId != userId)
				throw new AccessDeniedException();

			db.Feedback.Remove(feedback);
			await db.SaveChangesAsync();
		}
	}
}
