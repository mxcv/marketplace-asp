using Microsoft.AspNetCore.Identity;

namespace Marketplace.Models
{
	public class User : IdentityUser<int>
	{
		public string? Name { get; set; }
	}
}
