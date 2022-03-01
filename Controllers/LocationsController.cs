using System.Globalization;
using Marketplace.Models;
using Marketplace.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LocationsController : ControllerBase
	{
		private MarketplaceContext db;

		public LocationsController(MarketplaceContext db)
		{
			this.db = db;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			int languageId = (await db.Languages
				.Where(x => x.Code == CultureInfo.CurrentUICulture.ToString())
				.FirstOrDefaultAsync())?.Id ?? 1;

			return Ok(await db.Countries
				.Include(x => x.Names)
				.Include(x => x.Regions)
					.ThenInclude(x => x.Names)
				.Include(x => x.Regions)
					.ThenInclude(x => x.Cities)
						.ThenInclude(x => x.Names)
				.Select(x => new CountryModel() {
					Id = x.Id,
					Name = x.Names.Where(n => n.LanguageId == languageId).First().Value,
					Regions = x.Regions.Select(r => new RegionModel() {
						Id = r.Id,
						Name = r.Names.Where(n => n.LanguageId == languageId).First().Value,
						Cities = r.Cities.Select(c => new CityModel() {
							Id = c.Id,
							Name = c.Names.Where(n => n.LanguageId == languageId).First().Value
						})
					})
				})
				.ToListAsync()
			);
		}
	}
}
