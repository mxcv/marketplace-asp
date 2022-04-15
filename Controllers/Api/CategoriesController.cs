using System.Globalization;
using Marketplace.Dto;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly MarketplaceDbContext db;

		public CategoriesController(MarketplaceDbContext db)
		{
			this.db = db;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			int languageId = (await db.Languages
				.Where(x => x.Code == CultureInfo.CurrentUICulture.ToString())
				.FirstOrDefaultAsync())
				?.Id ?? 1;

			return Ok(await db.CategoryTitles
				.Where(x => x.LanguageId == languageId)
				.OrderBy(x => x.Value)
				.Select(x => new CategoryDto() {
					Id = x.CategoryId,
					Title = x.Value
				})
				.ToListAsync()
			);
		}
	}
}
