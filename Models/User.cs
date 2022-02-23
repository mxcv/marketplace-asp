using Microsoft.AspNetCore.Identity;

namespace Marketplace.Models
{
	public class User : IdentityUser<int>
	{
		public int? CityId { get; set; }

		public string? Name { get; set; }
		public DateTime Created { get; set; }

		public virtual City? City { get; set; }
		public virtual RefreshToken? RefreshToken { get; set; }
		public virtual ICollection<Item> Items { get; set; } = null!;
	}
}
