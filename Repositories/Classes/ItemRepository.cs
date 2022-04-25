using System.Security.Claims;
using System.Security.Principal;
using Marketplace.Dto;
using Marketplace.Exceptions;
using Marketplace.Models;
using Marketplace.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
	public partial class ItemRepository : IItemRepository
	{
		private readonly MarketplaceDbContext db;
		private readonly IImageRepository imageRepository;
		private readonly int? userId;

		public ItemRepository(MarketplaceDbContext db, IImageRepository imageRepository, IPrincipal principal)
		{
			this.db = db;
			this.imageRepository = imageRepository;

			string? identifier = ((ClaimsPrincipal)principal).FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (identifier != null)
				userId = int.Parse(identifier);
		}

		public async Task<ItemDto> GetItemAsync(int id)
		{
			Item? item = await db.Items
				.Include(x => x.Price)
				.Include(x => x.User)
					.ThenInclude(x => x.Image)
						.ThenInclude(x => x!.File)
				.Include(x => x.User)
					.ThenInclude(x => x.ReceivedFeedback)
				.Include(x => x.Images)
					.ThenInclude(x => x.File)
				.Where(x => x.Id == id)
				.AsSplitQuery()
				.FirstOrDefaultAsync();
			if (item == null)
				throw new NotFoundException();

			return GetDtoFromModel(item);
		}

		public async Task<IndexViewModel> GetItemsAsync(FilterViewModel filter, SortType? sortType, int pageIndex, int pageSize)
		{
			IQueryable<Item> items = db.Items
				.Include(x => x.Price)
				.Include(x => x.User)
					.ThenInclude(x => x.Image)
						.ThenInclude(x => x!.File)
				.Include(x => x.Images)
					.ThenInclude(x => x.File);

			if (filter.Query != null)
				items = items.Where(x => EF.Functions.Like(x.Title, $"%{filter.Query}%"));
			if (filter.MinPrice != null)
				items = items.Where(x => x.Price != null && x.Price.Value >= filter.MinPrice.Value);
			if (filter.MaxPrice != null)
				items = items.Where(x => x.Price != null && x.Price.Value <= filter.MaxPrice.Value);
			if (filter.CategoryId != null)
				items = items.Where(x => x.CategoryId == filter.CategoryId);
			if (userId != null)
				items = items.Where(x => x.UserId == userId);

			if (filter.CityId != null)
				items = items.Where(x => x.User.CityId == filter.CityId);
			else if (filter.RegionId != null)
				items = items
					.Include(x => x.User)
						.ThenInclude(x => x.City)
					.Where(x => x.User.City != null && x.User.City.RegionId == filter.RegionId);
			else if (filter.CountryId != null)
				items = items
					.Include(x => x.User)
						.ThenInclude(x => x.City)
							.ThenInclude(x => x!.Region)
					.Where(x => x.User.City != null && x.User.City.Region.CountryId == filter.CountryId);

			items = sortType switch {
				SortType.PriceAscending => items.OrderBy(x => x.Price == null ? decimal.MaxValue : x.Price.Value),
				SortType.PriceDescending => items.OrderByDescending(x => x.Price == null ? decimal.MinValue : x.Price.Value),
				_ => items.OrderByDescending(x => x.Created),
			};

			var list = await PaginatedList<ItemDto>.CreateAsync(items.Select(x => GetDtoFromModel(x)), pageIndex, pageSize);
			return new IndexViewModel(list, filter, sortType);
		}

		public async Task<int> AddItemAsync(ApiItemViewModel model)
		{
			if (userId == null)
				throw new UnauthorizedUserException();

			Item item = new Item() {
				Title = model.Title,
				Description = model.Description,
				Created = DateTime.UtcNow,
				UserId = userId.Value,
				CategoryId = model.Category?.Id,
				Price = model.Price == null ? null : new Price() {
					Value = model.Price.Value,
					CurrencyId = model.Price.Value == 0 || model.Currency == null
						? null
						: model.Currency.Id
				}
			};

			db.Items.Add(item);
			await db.SaveChangesAsync();
			return item.Id;
		}

		public async Task<int> AddItemAsync(ApiItemViewModel model, IFormFileCollection images)
		{
			int id = await AddItemAsync(model);
			try
			{
				await imageRepository.AddItemImagesAsync(id, images);
				return id;
			}
			catch
			{
				try
				{
					await RemoveItemAsync(id);
				}
				catch { }
				throw;
			}
		}

		public async Task RemoveItemAsync(int id)
		{
			await RemoveItemWithoutSaving(id);
			await db.SaveChangesAsync();
		}

		public async Task RemoveItemRangeAsync(IEnumerable<int> id)
		{
			foreach (int i in id)
				await RemoveItemWithoutSaving(i);
			await db.SaveChangesAsync();
		}

		private async Task RemoveItemWithoutSaving(int id)
		{
			if (userId == null)
				throw new UnauthorizedUserException();

			Item? item = await db.Items.Where(x => x.Id == id).FirstOrDefaultAsync();
			if (item == null)
				throw new NotFoundException();
			if (item.UserId != userId)
				throw new AccessDeniedException();

			await imageRepository.RemoveItemFileImagesAsync(id);
			db.Items.Remove(item);
		}

		private static ItemDto GetDtoFromModel(Item item)
		{
			return new ItemDto() {
				Id = item.Id,
				Title = item.Title,
				Description = item.Description,
				Created = item.Created,
				Category = item.CategoryId == null ? null : new CategoryDto() {
					Id = item.CategoryId.Value
				},
				Price = item.Price?.Value,
				Currency = item.Price == null || item.Price.CurrencyId == null ? null
					: new CurrencyDto() {
						Id = item.Price.CurrencyId.Value
					},
				User = new UserDto() {
					Id = item.UserId,
					PhoneNumber = item.User.PhoneNumber,
					Name = item.User.Name,
					Created = item.User.Created,
					FeedbackStatistics = item.User.ReceivedFeedback == null ? null : new FeedbackStatisticsDto(
						item.User.ReceivedFeedback.Count,
						item.User.ReceivedFeedback.Average(x => x.Rate)
					),
					City = item.User.CityId == null ? null : new CityDto() {
						Id = item.User.CityId.Value
					},
					Image = item.User.Image == null ? null : new ImageDto() {
						Path = ImageRepository.GetRelativeWebPath(item.User.Image.File.Name)
					}
				},
				Images = item.Images.Select(i => new ImageDto() {
					Path = ImageRepository.GetRelativeWebPath(i.File.Name)
				})
			};
		}
	}
}
