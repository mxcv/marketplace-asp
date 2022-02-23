namespace Marketplace.Models.DTO
{
	public class RegionModel
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public IEnumerable<CityModel>? Cities { get; set; }
		public CountryModel? Country { get; set; }
	}
}
