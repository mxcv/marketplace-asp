namespace Marketplace.Models
{
	public class Region
	{
		public int Id { get; set; }
		public int CountryId { get; set; }

		public virtual Country Country { get; set; } = null!;
		public virtual ICollection<RegionName> Names { get; set; } = null!;
		public virtual ICollection<City> Cities { get; set; } = null!;
	}
}
