using Marketplace.Dto;

namespace Marketplace.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    }
}
