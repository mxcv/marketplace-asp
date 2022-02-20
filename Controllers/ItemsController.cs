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
		[HttpPost("my")]
		public async Task<IActionResult> Get(PageInputModel pageInputModel)
		{
			int userId = GetUserId();
			var items = db.Items
				.Where(x => x.UserId == userId)
				.Include(x => x.Price)
				.ThenInclude(x => x == null ? null : x.Currency);
			int leftCount = await items.CountAsync() - pageInputModel.SkipCount - pageInputModel.TakeCount;
			if (leftCount < 0)
				leftCount = 0;

			return Ok(new PageOutputModel(
				await items
					.Select(x => new ItemModel() {
						Id = x.Id,
						Title = x.Title,
						Description = x.Description,
						Created = x.Created,
						Price = x.Price == null ? null : x.Price.Value,
						Currency = x.Price == null || x.Price.Currency == null ? null
							: new CurrencyModel() {
								Id = x.Price.Currency.Id,
								CountryCode = x.Price.Currency.CountryCode
							}
					})
					.Skip(pageInputModel.SkipCount)
					.Take(pageInputModel.TakeCount)
					.ToListAsync(),
				leftCount
			));
		}

		[Authorize]
		[HttpPut]
		public async Task<IActionResult> Put(ItemModel itemModel)
		{
			await db.Items.AddAsync(new Item() {
				Title = itemModel.Title,
				Description = itemModel.Description,
				Created = DateTime.UtcNow,
				UserId = GetUserId(),
				Price = itemModel.Price == null ? null : new Price() {
					Value = itemModel.Price.Value,
					CurrencyId = itemModel.Price.Value == 0 || itemModel.Currency == null
						? null
						: itemModel.Currency.Id
				}
			});
			await db.SaveChangesAsync();
			return Ok();
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
