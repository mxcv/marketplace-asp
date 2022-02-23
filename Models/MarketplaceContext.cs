using System.Text.Json;
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
			var languages = GetSeed<Language>(nameof(Languages));
			builder.Entity<Language>().HasData(languages);
			builder.Entity<Currency>().HasData(GetSeed<Currency>(nameof(Currencies)));
			builder.Entity<Category>().HasData(GetSeed<Category>(nameof(Categories)));
			foreach (Language language in languages)
			{
				builder.Entity<CategoryTitle>().HasData(GetSeed<CategoryTitle>(nameof(CategoryTitles), language.Code));
			}

			base.OnModelCreating(builder);
		}

		private IList<T> GetSeed<T>(string file)
		{
			return JsonSerializer.Deserialize<IList<T>>(
				File.ReadAllText(string.Format("DbSeed\\{0}.json", file)),
				new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
			)
				?? throw new NullReferenceException();
		}

		private IList<T> GetSeed<T>(string file, string language)
		{
			return GetSeed<T>(string.Format("{0}.{1}", file, language));
		}

		public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
		public DbSet<Currency> Currencies => Set<Currency>();
		public DbSet<Price> Prices => Set<Price>();
		public DbSet<Item> Items => Set<Item>();
		public DbSet<Language> Languages => Set<Language>();
		public DbSet<Category> Categories => Set<Category>();
		public DbSet<CategoryTitle> CategoryTitles => Set<CategoryTitle>();

	}
}
