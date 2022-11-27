using Marketplace.Dto;

namespace Marketplace.Repositories.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync();
    }
}
