using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Models
{
	public class Price
	{
		public int Id { get; set; }
		public int? CurrencyId { get; set; }
		public int ItemId { get; set; }

		[Column(TypeName = "DECIMAL(18,2)")]
		public decimal Value { get; set; }

		public virtual Currency? Currency { get; set; } = null!;
		public virtual Item Item { get; set; } = null!;
	}
}
