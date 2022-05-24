using System.Security.Claims;
using System.Security.Principal;
using Marketplace.Dto;
using Marketplace.Exceptions;
using Marketplace.Models;
using Marketplace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly MarketplaceDbContext db;
		private readonly UserManager<User> userManager;
		private readonly IImageRepository imageRepository;
		private readonly int? userId;

		public UserRepository(MarketplaceDbContext db, UserManager<User> userManager, IImageRepository imageRepository, IPrincipal principal)
		{
			this.db = db;
			this.userManager = userManager;
			this.imageRepository = imageRepository;

			string? identifier = ((ClaimsPrincipal)principal).FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (identifier != null)
				userId = int.Parse(identifier);
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

		public async Task<UserDto> GetCurrentUser()
		{
			if (userId == null)
				throw new UnauthorizedUserException();

			return await GetUser(userId.Value);
		}

		public async Task<IEnumerable<UserDto>> GetModerators()
		{
			int roleId = await db.Roles
				.Where(x => x.Name == "Moderator")
				.Select(x => x.Id)
				.FirstAsync();

			var userIds = await db.UserRoles
				.Where(x => x.RoleId == roleId)
				.Select(x => x.UserId)
				.ToListAsync();

			return await db.Users.Where(x => userIds.Contains(x.Id))
				.Select(x => new UserDto() {
					Id = x.Id,
					Email = x.Email,
					Created = x.Created
				})
				.ToListAsync();
		}

		public async Task<int> AddModerator(ModeratorRegisterViewModel model)
		{
			User user = new User() {
				UserName = model.Email,
				Email = model.Email,
				Created = DateTime.UtcNow
			};
			var result = await userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				throw new ModelException(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));

			await userManager.AddToRoleAsync(user, "Moderator");
			return user.Id;
		}

		public async Task RemoveModerator(int id)
		{
			User? user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
			if (user == null || !await userManager.IsInRoleAsync(user, "Moderator"))
				throw new NotFoundException();
			db.Users.Remove(user);
			await db.SaveChangesAsync();
		}

		public async Task<int> AddSeller(ApiRegisterViewModel model)
		{
			User user = new User() {
				UserName = model.Email,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				Name = model.Name,
				Created = DateTime.UtcNow,
				CityId = model.City?.Id
			};
			var result = await userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				throw new ModelException(string.Join(Environment.NewLine, result.Errors.Select(x => x.Description)));

			await userManager.AddToRoleAsync(user, "Seller");
			return user.Id;
		}

		public async Task<int> AddSeller(ApiRegisterViewModel model, IFormFile image)
		{
			int id = await AddSeller(model);
			try
			{
				await imageRepository.SetUserImageAsync(image, id);
			}
			catch { }

			return id;
		}
	}
}
