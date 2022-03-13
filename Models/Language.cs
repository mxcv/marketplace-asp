namespace Marketplace.Models
{
	public class Language
	{
		public int Id { get; set; }

		public string Code { get; set; } = null!;

		public virtual ICollection<CategoryTitle> CategoryTitles { get; set; } = null!;
		public virtual ICollection<CountryName> CountryNames { get; set; } = null!;
		public virtual ICollection<RegionName> RegionNames { get; set; } = null!;
		public virtual ICollection<CityName> CityNames { get; set; } = null!;
	}
}
