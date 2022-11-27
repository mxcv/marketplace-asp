using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Marketplace.Repositories.Classes
{
    public class LocationRepository : ILocationRepository
	{
		private readonly MarketplaceDbContext db;

		public LocationRepository(MarketplaceDbContext db)
		{
			this.db = db;
		}

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync()
        {
			int languageId = (await db.Languages
				.Where(x => x.Code == CultureInfo.CurrentUICulture.ToString())
				.FirstOrDefaultAsync())
				?.Id ?? 1;

			return (await db.Countries
				.Include(x => x.Names.Where(n => n.LanguageId == languageId).Take(1))
				.Include(x => x.Regions)
					.ThenInclude(x => x.Names.Where(n => n.LanguageId == languageId).Take(1))
				.Include(x => x.Regions)
					.ThenInclude(x => x.Cities)
						.ThenInclude(x => x.Names.Where(n => n.LanguageId == languageId).Take(1))
				.ToListAsync())
				.Select(x => new CountryDto()
				{
					Id = x.Id,
					Name = x.Names.Single().Value,
					Regions = x.Regions.Select(r => new RegionDto()
					{
						Id = r.Id,
						Name = r.Names.Single().Value,
						Cities = r.Cities.Select(c => new CityDto()
						{
							Id = c.Id,
							Name = c.Names.Single().Value
						})
						.OrderBy(x => x.Name)
					})
					.OrderBy(x => x.Name)
				})
				.OrderBy(x => x.Name);
		}
    }
}
