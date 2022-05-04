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
					.ThenInclude(x => x.Image)
						.ThenInclude(x => x!.File)
				.Where(x => x.ReviewerId != userId && x.SellerId == sellerId)
				.OrderByDescending(x => x.Created)
				.Select(x => GetDtoFromModel(x));

			return await PaginatedList<FeedbackDto>.CreateAsync(feedback, pageIndex, pageSize);
		}

		public async Task<FeedbackDto?> GetLeftFeedbackAsync(int sellerId)
		{
			if (userId == null)
				return null;

			return await db.Feedback
				.Include(x => x.Reviewer)
					.ThenInclude(x => x.Image)
						.ThenInclude(x => x!.File)
				.Where(x => x.ReviewerId == userId && x.SellerId == sellerId)
				.Select(x => GetDtoFromModel(x))
				.FirstOrDefaultAsync();
		}

		public async Task<int> AddFeedbackAsync(ApiFeedbackViewModel model)
		{
			if (userId == null)
				throw new UnauthorizedUserException();
			if (userId == model.Seller.Id || db.Feedback.Where(x => x.SellerId == model.Seller.Id && x.ReviewerId == userId).Any())
				throw new ModelException();

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

		public async Task RemoveFeedbackAsync(int sellerId)
		{
			if (userId == null)
				throw new UnauthorizedUserException();

			Feedback? feedback = await db.Feedback.Where(x => x.ReviewerId == userId && x.SellerId == sellerId).FirstOrDefaultAsync();
			if (feedback != null)
			{
				db.Feedback.Remove(feedback);
				await db.SaveChangesAsync();
			}
		}

		private static FeedbackDto GetDtoFromModel(Feedback feedback)
		{
			return new FeedbackDto() {
				Id = feedback.Id,
				Rate = feedback.Rate,
				Text = feedback.Text,
				Created = feedback.Created,
				Reviewer = new UserDto() {
					Id = feedback.Reviewer.Id,
					PhoneNumber = feedback.Reviewer.PhoneNumber,
					Name = feedback.Reviewer.Name,
					Created = feedback.Reviewer.Created,
					City = feedback.Reviewer.CityId == null ? null : new CityDto() {
						Id = feedback.Reviewer.CityId.Value
					},
					Image = feedback.Reviewer.Image == null ? null : new ImageDto() {
						Path = ImageRepository.GetRelativeWebPath(feedback.Reviewer.Image.File.Name)
					}
				}
			};
		}
	}
}
