using Marketplace.DTO;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public partial class CurrenciesController : ControllerBase
	{
		private MarketplaceDbContext db;

		public CurrenciesController(MarketplaceDbContext db)
		{
			this.db = db;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await db.Currencies
				.Select(x => new CurrencyDto() {
					Id = x.Id,
					LanguageTag = x.LanguageTag
				})
				.OrderBy(x => x.Id)
				.ToListAsync()
			);
		}
	}
}
