using System.Globalization;
using Marketplace.Models;
using Marketplace.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private MarketplaceContext db;

		public CategoriesController(MarketplaceContext db)
		{
			this.db = db;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			int? languageId = (await db.Languages
				.Where(x => x.Code == CultureInfo.CurrentUICulture.ToString())
				.FirstOrDefaultAsync()
				)?.Id;
			if (languageId == null)
				languageId = 1;

			return Ok(await db.CategoryTitles
				.Where(x => x.LanguageId == languageId)
				.Select(x => new CategoryModel() {
					Id = x.CategoryId,
					Title = x.Value
				})
				.ToListAsync()
			);
		}
	}
}
