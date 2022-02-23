using Marketplace.Models;
using Marketplace.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public partial class CurrenciesController : ControllerBase
	{
		private MarketplaceContext db;

		public CurrenciesController(MarketplaceContext db)
		{
			this.db = db;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await db.Currencies
				.Select(x => new CurrencyModel() {
					Id = x.Id,
					LanguageTag = x.LanguageTag
				})
				.OrderBy(x => x.Id)
				.ToListAsync()
			);
		}
	}
}
