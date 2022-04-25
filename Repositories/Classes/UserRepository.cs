using Marketplace.Dto;
using Marketplace.Exceptions;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly MarketplaceDbContext db;

		public UserRepository(MarketplaceDbContext db)
		{
			this.db = db;
		}

		public async Task<UserDto> GetUser(int id)
		{
			User? user = await db.Users
				.Include(x => x.Image)
					.ThenInclude(x => x!.File)
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();
			if (user == null)
				throw new NotFoundException();

			int feedbackCount = await db.Feedback.Where(x => x.SellerId == id).CountAsync();
			double feedbackAverage = feedbackCount == 0 ? 0 : await db.Feedback.Where(x => x.SellerId == id).AverageAsync(x => x.Rate);

			return new UserDto() {
				Id = user.Id,
				PhoneNumber = user.PhoneNumber,
				Name = user.Name,
				Created = user.Created,
				FeedbackStatistics = new FeedbackStatisticsDto(feedbackCount, feedbackAverage),
				City = user.CityId == null ? null : new CityDto() {
					Id = user.CityId.Value
				},
				Image = user.Image == null ? null : new ImageDto() {
					Path = ImageRepository.GetRelativeWebPath(user.Image.File.Name)
				}
			};
		}
	}
}
