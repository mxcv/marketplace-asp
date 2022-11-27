using Marketplace.Dto;

namespace Marketplace.Repositories
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<CurrencyDto>> GetCurrenciesAsync();
    }
}
