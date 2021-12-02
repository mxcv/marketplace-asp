using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Models
{
	public class MarketplaceContext : IdentityDbContext<User, IdentityRole<int>, int>
	{
		public MarketplaceContext(DbContextOptions<MarketplaceContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}
