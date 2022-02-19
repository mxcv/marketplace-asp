using Microsoft.AspNetCore.Identity;

namespace Marketplace.Models
{
	public class User : IdentityUser<int>
	{
		public string? Name { get; set; }
		public DateTime Created { get; set; }

		public virtual RefreshToken? RefreshToken { get; set; }
		public virtual ICollection<Item> Items { get; set; } = null!;
	}
}
