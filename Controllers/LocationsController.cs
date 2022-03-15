using System.Globalization;
using Marketplace.Dto;
using Marketplace.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LocationsController : ControllerBase
	{
		private MarketplaceDbContext db;

		public LocationsController(MarketplaceDbContext db)
		{
			this.db = db;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			int languageId = (await db.Languages
				.Where(x => x.Code == CultureInfo.CurrentUICulture.ToString())
				.FirstOrDefaultAsync())?.Id ?? 1;

			return Ok((await db.Countries
				.Include(x => x.Names.Where(n => n.LanguageId == languageId).Take(1))
				.Include(x => x.Regions)
					.ThenInclude(x => x.Names.Where(n => n.LanguageId == languageId).Take(1))
				.Include(x => x.Regions)
					.ThenInclude(x => x.Cities)
						.ThenInclude(x => x.Names.Where(n => n.LanguageId == languageId).Take(1))
				.ToListAsync())
				.Select(x => new CountryDto() {
					Id = x.Id,
					Name = x.Names.Single().Value,
					Regions = x.Regions.Select(r => new RegionDto() {
						Id = r.Id,
						Name = r.Names.Single().Value,
						Cities = r.Cities.Select(c => new CityDto() {
							Id = c.Id,
							Name = c.Names.Single().Value
						})
						.OrderBy(x => x.Name)
					})
					.OrderBy(x => x.Name)
				})
				.OrderBy(x => x.Name)
			);
		}
	}
}
