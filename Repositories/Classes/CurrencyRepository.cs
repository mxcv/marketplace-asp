using Marketplace.Dto;
using Marketplace.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Marketplace.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
		private readonly MarketplaceDbContext db;

		public CurrencyRepository(MarketplaceDbContext db)
		{
			this.db = db;
		}

		public async Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync()
        {
			return await db.Currencies
				.Select(x => new CurrencyDto()
				{
					Id = x.Id,
					LanguageTag = x.LanguageTag,
					Code = new RegionInfo(x.LanguageTag).ISOCurrencySymbol,
					Symbol = new CultureInfo(x.LanguageTag).NumberFormat.CurrencySymbol
				})
				.OrderBy(x => x.Id)
				.ToListAsync();
		}
    }
}
