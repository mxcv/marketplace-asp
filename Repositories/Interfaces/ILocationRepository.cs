using Marketplace.Dto;

namespace Marketplace.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<CountryDto>> GetCountriesAsync();
    }
}
