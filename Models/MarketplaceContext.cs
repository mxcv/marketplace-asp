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

		protected override void OnModelCreating(ModelBuilder builder)
		{
			string[] countryCodes = { "en-US", "de-DE", "uk-UA" };
			builder.Entity<Currency>()
				.HasData(countryCodes.Select((c, i) => new Currency() { Id = i + 1, CountryCode = c }));

			base.OnModelCreating(builder);
		}

		public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
		public DbSet<Currency> Currencies => Set<Currency>();
		public DbSet<Price> Prices => Set<Price>();
		public DbSet<Item> Items => Set<Item>();
	}
}
