using System.Security.Claims;
using Marketplace.DTO;
using Marketplace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public partial class ItemsController : ControllerBase
	{
		private MarketplaceDbContext db;

		public ItemsController(MarketplaceDbContext db)
		{
			this.db = db;
		}

		[HttpGet]
		public async Task<IActionResult> Get(
			string? query,
			decimal? minPrice,
			decimal? maxPrice,
			int? categoryId,
			int? countryId,
			int? regionId,
			int? cityId,
			int? userId,
			int? sortType,
			int? skipCount,
			int? takeCount)
		{
			IQueryable<Item> items = db.Items
				.Include(x => x.Price)
					.ThenInclude(x => x!.Currency)
				.Include(x => x.Category)
				.Include(x => x.User)
					.ThenInclude(x => x.Image)
						.ThenInclude(x => x!.File)
				.Include(x => x.User)
					.ThenInclude(x => x.City)
						.ThenInclude(x => x!.Region)
							.ThenInclude(x => x.Country)
				.Include(x => x.Images)
					.ThenInclude(x => x.File);

			if (query != null)
				items = items.Where(x => EF.Functions.Like(x.Title, $"%{query}%"));
			if (minPrice != null)
				items = items.Where(x => x.Price != null && x.Price.Value >= minPrice.Value);
			if (maxPrice != null)
				items = items.Where(x => x.Price != null && x.Price.Value <= maxPrice.Value);
			if (categoryId != null)
				items = items.Where(x => x.CategoryId == categoryId);
			if (userId != null)
				items = items.Where(x => x.UserId == userId);
			if (cityId != null)
				items = items.Where(x => x.User.CityId == cityId);
			else if (regionId != null)
				items = items.Where(x => x.User.City != null && x.User.City.RegionId == regionId);
			else if (countryId != null)
				items = items.Where(x => x.User.City != null && x.User.City.Region.CountryId == countryId);
			if (sortType != null)
			{
				switch ((SortType)sortType)
				{
					case SortType.CreatedDescending:
						items = items.OrderByDescending(x => x.Created);
						break;
					case SortType.PriceAscending:
						items = items.OrderBy(x => x.Price == null).ThenBy(x => x.Price);
						break;
					case SortType.PriceDescending:
						items = items.OrderBy(x => x.Price == null).ThenByDescending(x => x.Price);
						break;
				}
			}

			int leftCount = 0;
			if (takeCount != null)
				leftCount = await items.CountAsync() - takeCount.Value;
			if (skipCount != null)
				leftCount -= skipCount.Value;
			if (leftCount < 0)
				leftCount = 0;

			if (skipCount != null)
				items = items.Skip(skipCount.Value);
			if (takeCount != null)
				items = items.Take(takeCount.Value);

			var itemModels = items.Select(x => new ItemDto() {
				Id = x.Id,
				Title = x.Title,
				Description = x.Description,
				Created = x.Created,
				Category = x.Category == null ? null : new CategoryDto() {
					Id = x.Category.Id
				},
				Price = x.Price == null ? null : x.Price.Value,
				Currency = x.Price == null || x.Price.Currency == null ? null
					: new CurrencyDto() {
						Id = x.Price.Currency.Id,
						LanguageTag = x.Price.Currency.LanguageTag
					},
				User = new UserDto() {
					PhoneNumber = x.User.PhoneNumber,
					Name = x.User.Name,
					Created = x.User.Created,
					City = x.User.City == null ? null : new CityDto() {
						Id = x.User.City.Id
					},
					Image = x.User.Image == null ? null : new ImageDto() {
						Path = string.Format("/{0}/{1}", ImagesController.DirectoryPath, x.User.Image.File.Name)
					}
				},
				Images = x.Images.Select(i => new ImageDto() {
					Path = string.Format("/{0}/{1}", ImagesController.DirectoryPath, i.File.Name)
				})
			});

			return Ok(new PageDto(await itemModels.ToListAsync(), leftCount));
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Post(ItemDto itemDto)
		{
			Item item = new Item() {
				Title = itemDto.Title,
				Description = itemDto.Description,
				Created = DateTime.UtcNow,
				UserId = GetUserId(),
				CategoryId = itemDto.Category?.Id,
				Price = itemDto.Price == null ? null : new Price() {
					Value = itemDto.Price.Value,
					CurrencyId = itemDto.Price.Value == 0 || itemDto.Currency == null
						? null
						: itemDto.Currency.Id
				}
			};

			db.Items.Add(item);
			await db.SaveChangesAsync();
			return Ok(item.Id);
		}

		[Authorize]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			int userId = GetUserId();
			Item? item = await db.Items.Where(x => x.Id == id).FirstOrDefaultAsync();
			if (item == null || item.UserId != userId)
				return BadRequest();

			db.Items.Remove(item);
			await db.SaveChangesAsync();
			return Ok();
		}

		private int GetUserId()
		{
			return int.Parse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());
		}
	}
}
