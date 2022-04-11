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
		private MarketplaceDbContext db;
		private IImageRepository imageRepository;
		private int? userId;

		public ItemRepository(MarketplaceDbContext db, IImageRepository imageRepository, IPrincipal principal)
		{
			this.db = db;
			this.imageRepository = imageRepository;

			string? identifier = ((ClaimsPrincipal)principal).FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (identifier != null)
				userId = int.Parse(identifier);
		}

		public async Task<IndexViewModel> GetItems(IndexViewModel model)
		{
			IQueryable<Item> items = db.Items
				.Include(x => x.Price)
				.Include(x => x.User)
					.ThenInclude(x => x.Image)
						.ThenInclude(x => x!.File)
				.Include(x => x.Images)
					.ThenInclude(x => x.File);

			if (model.Filter != null)
			{
				if (model.Filter.Query != null)
					items = items.Where(x => EF.Functions.Like(x.Title, $"%{model.Filter.Query}%"));
				if (model.Filter.MinPrice != null)
					items = items.Where(x => x.Price != null && x.Price.Value >= model.Filter.MinPrice.Value);
				if (model.Filter.MaxPrice != null)
					items = items.Where(x => x.Price != null && x.Price.Value <= model.Filter.MaxPrice.Value);
				if (model.Filter.CategoryId != null)
					items = items.Where(x => x.CategoryId == model.Filter.CategoryId);
				if (userId != null)
					items = items.Where(x => x.UserId == userId);

				if (model.Filter.CityId != null)
					items = items.Where(x => x.User.CityId == model.Filter.CityId);
				else if (model.Filter.RegionId != null)
					items = items
						.Include(x => x.User)
							.ThenInclude(x => x.City)
						.Where(x => x.User.City != null && x.User.City.RegionId == model.Filter.RegionId);
				else if (model.Filter.CountryId != null)
					items = items
						.Include(x => x.User)
							.ThenInclude(x => x.City)
								.ThenInclude(x => x!.Region)
						.Where(x => x.User.City != null && x.User.City.Region.CountryId == model.Filter.CountryId);
			}

			switch (model.SortType)
			{
				default:
				case SortType.CreatedDescending:
					items = items.OrderByDescending(x => x.Created);
					break;
				case SortType.PriceAscending:
					items = items.OrderBy(x => x.Price == null ? decimal.MaxValue : x.Price.Value);
					break;
				case SortType.PriceDescending:
					items = items.OrderByDescending(x => x.Price == null ? decimal.MinValue : x.Price.Value);
					break;
			}

			if (model.Page != null)
			{
				model.Page.TotalItems = await items.CountAsync();
				model.Page.TotalPages = (int)Math.Ceiling(model.Page.TotalItems / (double)model.Page.Size);
				items = items.Skip((model.Page.Index - 1) * model.Page.Size).Take(model.Page.Size);
			}

			var itemModels = items.Select(x => new ItemDto() {
				Id = x.Id,
				Title = x.Title,
				Description = x.Description,
				Created = x.Created,
				Category = x.CategoryId == null ? null : new CategoryDto() {
					Id = x.CategoryId.Value
				},
				Price = x.Price == null ? null : x.Price.Value,
				Currency = x.Price == null || x.Price.CurrencyId == null ? null
					: new CurrencyDto() {
						Id = x.Price.CurrencyId.Value
					},
				User = new UserDto() {
					Id = x.UserId,
					PhoneNumber = x.User.PhoneNumber,
					Name = x.User.Name,
					Created = x.User.Created,
					City = x.User.CityId == null ? null : new CityDto() {
						Id = x.User.CityId.Value
					},
					Image = x.User.Image == null ? null : new ImageDto() {
						Path = imageRepository.GetRelativeWebPath(x.User.Image.File.Name)
					}
				},
				Images = x.Images.Select(i => new ImageDto() {
					Path = imageRepository.GetRelativeWebPath(i.File.Name)
				})
			});

			model.Items = await itemModels.ToListAsync();
			return model;
		}

		public async Task<IndexViewModel> GetMyItems(IndexViewModel model)
		{
			if (model.Filter == null)
				model.Filter = new FilterViewModel();
			model.Filter.UserId = userId;
			return await GetItems(model);
		}

		public async Task<int> AddItem(ApiItemViewModel model)
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

		public async Task<int> AddItem(ApiItemViewModel model, IFormFileCollection images)
		{
			int id = await AddItem(model);
			try
			{
				await imageRepository.AddItemImagesAsync(id, images);
				return id;
			}
			catch
			{
				try
				{
					await RemoveItem(id);
				}
				catch { }
				throw;
			}
		}

		public async Task RemoveItem(int id)
		{
			if (userId == null)
				throw new UnauthorizedUserException();

			Item? item = await db.Items
				.Include(x => x.Images)
					.ThenInclude(x => x.File)
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();
			if (item == null)
				throw new NotFoundException();
			if (item.UserId != userId)
				throw new AccessDeniedException();

			await imageRepository.RemoveItemImagesAsync(id);
			db.Items.Remove(item);
			await db.SaveChangesAsync();
		}
	}
}
