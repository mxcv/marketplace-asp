using System.Globalization;
using System.Security.Claims;
using Marketplace.Models;
using Marketplace.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public partial class ItemsController : ControllerBase
	{
		private MarketplaceContext db;

		public ItemsController(MarketplaceContext db)
		{
			this.db = db;
		}

		[Authorize]
		[HttpGet("my")]
		public async Task<IActionResult> GetMyItems(int? skipCount, int? takeCount)
		{
			int? languageId = (await db.Languages
				.Where(x => x.Code == CultureInfo.CurrentUICulture.ToString())
				.FirstOrDefaultAsync()
				)?.Id;
			if (languageId == null)
				languageId = 1;

			int userId = GetUserId();
			var items = db.Items
				.Where(x => x.UserId == userId)
				.Include(x => x.Category)
					.ThenInclude(x => x == null ? null : x.Titles)
				.Include(x => x.Price)
					.ThenInclude(x => x == null ? null : x.Currency)
				.Include(x => x.Images)
					.ThenInclude(x => x.File);

			int leftCount = 0;
			if (takeCount.HasValue)
				leftCount = await items.CountAsync() - takeCount.Value;
			if (skipCount.HasValue)
				leftCount -= skipCount.Value;
			if (leftCount < 0)
				leftCount = 0;

			var itemModels = items.Select(x => new ItemModel() {
				Id = x.Id,
				Title = x.Title,
				Description = x.Description,
				Created = x.Created,
				Category = x.Category == null ? null : new CategoryModel() {
					Id = x.Category.Id,
					Title = x.Category.Titles.Where(x => x.LanguageId == languageId).First().Value
				},
				Price = x.Price == null ? null : x.Price.Value,
				Currency = x.Price == null || x.Price.Currency == null ? null
					: new CurrencyModel() {
						Id = x.Price.Currency.Id,
						LanguageTag = x.Price.Currency.LanguageTag
					},
				Images = x.Images.Select(i => new ImageModel() {
					Path = string.Format("/{0}/{1}", ImagesController.DirectoryPath, i.File.Name)
				})
			});

			if (skipCount.HasValue)
				itemModels = itemModels.Skip(skipCount.Value);
			if (takeCount.HasValue)
				itemModels = itemModels.Take(takeCount.Value);

			return Ok(new PageModel(await itemModels.ToListAsync(), leftCount));
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Post(ItemModel itemModel)
		{
			Item item = new Item() {
				Title = itemModel.Title,
				Description = itemModel.Description,
				Created = DateTime.UtcNow,
				UserId = GetUserId(),
				CategoryId = itemModel.Category?.Id,
				Price = itemModel.Price == null ? null : new Price() {
					Value = itemModel.Price.Value,
					CurrencyId = itemModel.Price.Value == 0 || itemModel.Currency == null
						? null
						: itemModel.Currency.Id
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
