using Marketplace.Dto;

namespace Marketplace.Repositories
{
    public interface ILocationRepository
    {
        Task<IEnumerable<CountryDto>> GetCountriesAsync();
    }
}
