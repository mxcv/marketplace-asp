namespace Marketplace.Models
{
	public class ExchangeInfoRequest
	{
		public int Id { get; set; }
		public int BaseCurrencyId { get; set; }

		public DateTimeOffset Created { get; set; }

		public virtual Currency BaseCurrency { get; set; } = null!;
	}
}
