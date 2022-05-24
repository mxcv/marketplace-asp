using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Models
{
	public class MarketplaceDbContext : IdentityDbContext<User, IdentityRole<int>, int>
	{
		public MarketplaceDbContext(DbContextOptions<MarketplaceDbContext> options) : base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<User>()
				.HasMany(x => x.ReceivedFeedback)
				.WithOne(x => x.Seller)
				.HasForeignKey(x => x.SellerId)
				.OnDelete(DeleteBehavior.ClientCascade);

			SeedDb(builder);

			base.OnModelCreating(builder);
		}

		private void SeedDb(ModelBuilder builder)
		{
			var languages = GetSeed<Language>(nameof(Languages));
			builder.Entity<Language>().HasData(languages);
			builder.Entity<Currency>().HasData(GetSeed<Currency>(nameof(Currencies)));
			builder.Entity<Category>().HasData(GetSeed<Category>(nameof(Categories)));
			builder.Entity<Country>().HasData(GetSeed<Country>(nameof(Countries)));
			builder.Entity<Region>().HasData(GetSeed<Region>(nameof(Regions)));
			builder.Entity<City>().HasData(GetSeed<City>(nameof(Cities)));

			foreach (Language language in languages)
			{
				builder.Entity<CategoryTitle>().HasData(GetSeed<CategoryTitle>(nameof(CategoryTitles), language.Code));
				builder.Entity<CountryName>().HasData(GetSeed<CountryName>(nameof(CountryNames), language.Code));
				builder.Entity<RegionName>().HasData(GetSeed<RegionName>(nameof(RegionNames), language.Code));
				builder.Entity<CityName>().HasData(GetSeed<CityName>(nameof(CityNames), language.Code));
			}
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
		public DbSet<Exchange> Exchanges => Set<Exchange>();
		public DbSet<ExchangeInfoRequest> ExchangeInfoRequests => Set<ExchangeInfoRequest>();
		public DbSet<Price> Prices => Set<Price>();
		public DbSet<Item> Items => Set<Item>();
		public DbSet<Language> Languages => Set<Language>();
		public DbSet<Category> Categories => Set<Category>();
		public DbSet<CategoryTitle> CategoryTitles => Set<CategoryTitle>();
		public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
		public DbSet<ItemImage> ItemImages => Set<ItemImage>();
		public DbSet<UserImage> UserImages => Set<UserImage>();
		public DbSet<Feedback> Feedback => Set<Feedback>();

		public DbSet<Country> Countries => Set<Country>();
		public DbSet<CountryName> CountryNames => Set<CountryName>();
		public DbSet<Region> Regions => Set<Region>();
		public DbSet<RegionName> RegionNames => Set<RegionName>();
		public DbSet<City> Cities => Set<City>();
		public DbSet<CityName> CityNames => Set<CityName>();
	}
}
