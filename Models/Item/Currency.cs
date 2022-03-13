using Microsoft.EntityFrameworkCore;

namespace Marketplace.Models
{
	[Index(nameof(LanguageTag), IsUnique = true)]
	public class Currency
	{
		public int Id { get; set; }

		public string LanguageTag { get; set; } = null!;

		public virtual ICollection<Price> Prices { get; set; } = null!;
	}
}
