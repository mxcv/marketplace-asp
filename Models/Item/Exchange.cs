namespace Marketplace.Models
{
	public class Exchange
	{
		public int Id { get; set; }
		public int CurrencyId { get; set; }

		public decimal Rate { get; set; }

		public virtual Currency Currency { get; set; } = null!;
	}
}
