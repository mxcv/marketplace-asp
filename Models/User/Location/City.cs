namespace Marketplace.Models
{
	public class City
	{
		public int Id { get; set; }
		public int RegionId { get; set; }

		public Region Region { get; set; } = null!;
		public virtual ICollection<CityName> Names { get; set; } = null!;
		public virtual ICollection<User> Users { get; set; } = null!;
	}
}
