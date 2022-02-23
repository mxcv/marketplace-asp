namespace Marketplace.Models.DTO
{
	public class CountryModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public IEnumerable<RegionModel>? Regions { get; set; }
	}
}
