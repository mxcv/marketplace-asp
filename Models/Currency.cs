using Microsoft.EntityFrameworkCore;

namespace Marketplace.Models
{
	[Index(nameof(CountryCode), IsUnique = true)]
	public class Currency
	{
		public int Id { get; set; }

		public string CountryCode { get; set; } = null!;

		public virtual ICollection<Price> Prices { get; set; } = null!;
	}
}
