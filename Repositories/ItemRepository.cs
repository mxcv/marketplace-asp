using System.Security.Claims;
using System.Security.Principal;
using Marketplace.Controllers;
using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Repositories
{
	public partial class ItemRepository : IItemRepository
	{
		private MarketplaceDbContext db;
		private IWebHostEnvironment appEnvironment;
		private int? userId;

		public ItemRepository(MarketplaceDbContext db, IWebHostEnvironment appEnvironment, IPrincipal principal)
		{
			this.db = db;
			this.appEnvironment = appEnvironment;

			string? identifier = ((ClaimsPrincipal)principal).FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (identifier != null)
				userId = int.Parse(identifier);
		}

		public async Task<PageDto> GetItems(ItemRequest request)
		{
			IQueryable<Item> items = db.Items
				.Include(x => x.Price)
				.Include(x => x.User)
					.ThenInclude(x => x.Image)
						.ThenInclude(x => x!.File)
				.Include(x => x.Images)
					.ThenInclude(x => x.File);

			if (request.Query != null)
				items = items.Where(x => EF.Functions.Like(x.Title, $"%{request.Query}%"));
			if (request.MinPrice != null)
				items = items.Where(x => x.Price != null && x.Price.Value >= request.MinPrice.Value);
			if (request.MaxPrice != null)
				items = items.Where(x => x.Price != null && x.Price.Value <= request.MaxPrice.Value);
			if (request.CategoryId != null)
				items = items.Where(x => x.CategoryId == request.CategoryId);
			if (userId != null)
				items = items.Where(x => x.UserId == userId);

			if (request.CityId != null)
				items = items.Where(x => x.User.CityId == request.CityId);
			else if (request.RegionId != null)
				items = items
					.Include(x => x.User)
						.ThenInclude(x => x.City)
					.Where(x => x.User.City != null && x.User.City.RegionId == request.RegionId);
			else if (request.CountryId != null)
				items = items
					.Include(x => x.User)
						.ThenInclude(x => x.City)
							.ThenInclude(x => x!.Region)
					.Where(x => x.User.City != null && x.User.City.Region.CountryId == request.CountryId);

			if (request.SortTypeId == null)
				request.SortTypeId = (int)default(SortType);
			switch ((SortType)request.SortTypeId)
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

			int leftCount = 0;
			if (request.TakeCount != null)
				leftCount = await items.CountAsync() - request.TakeCount.Value;
			if (request.SkipCount != null)
				leftCount -= request.SkipCount.Value;
			if (leftCount < 0)
				leftCount = 0;

			if (request.SkipCount != null)
				items = items.Skip(request.SkipCount.Value);
			if (request.TakeCount != null)
				items = items.Take(request.TakeCount.Value);

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
						Path = string.Format("/{0}/{1}", ImagesController.DirectoryPath, x.User.Image.File.Name)
					}
				},
				Images = x.Images.Select(i => new ImageDto() {
					Path = string.Format("/{0}/{1}", ImagesController.DirectoryPath, i.File.Name)
				})
			});

			return new PageDto(await itemModels.ToListAsync(), leftCount);
		}

		public async Task<PageDto> GetMyItems(ItemRequest request)
		{
			request.UserId = userId;
			return await GetItems(request);
		}

		public async Task<int?> AddItem(ItemViewModel model)
		{
			if (userId == null)
				return null;

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

		public async Task<bool> RemoveItem(int id)
		{
			if (userId == null)
				return false;

			Item? item = await db.Items
				.Include(x => x.Images)
					.ThenInclude(x => x.File)
				.Where(x => x.Id == id)
				.FirstOrDefaultAsync();
			if (item == null || item.UserId != userId)
				return false;

			foreach (ItemImage image in item.Images)
				File.Delete(Path.Combine(
					appEnvironment.WebRootPath,
					ImagesController.DirectoryPath,
					image.File.Name
				));

			db.Items.Remove(item);
			await db.SaveChangesAsync();
			return true;
		}
	}
}
