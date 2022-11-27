using Marketplace.Dto;
using Marketplace.Models;
using Marketplace.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Marketplace.Repositories.Classes
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MarketplaceDbContext db;

        public CategoryRepository(MarketplaceDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            int languageId = (await db.Languages
                .Where(x => x.Code == CultureInfo.CurrentUICulture.ToString())
                .FirstOrDefaultAsync())
                ?.Id ?? 1;

            return await db.CategoryTitles
                .Where(x => x.LanguageId == languageId)
                .OrderBy(x => x.Value)
                .Select(x => new CategoryDto() {
                    Id = x.CategoryId,
                    Title = x.Value
                })
                .ToListAsync();
        }
    }
}
