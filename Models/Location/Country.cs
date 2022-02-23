namespace Marketplace.Models
{
	public class Country
	{
		public int Id { get; set; }

		public virtual ICollection<CountryName> Names { get; set; } = null!;
		public virtual ICollection<Region> Regions { get; set; } = null!;
	}
}
