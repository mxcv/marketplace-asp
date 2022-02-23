namespace Marketplace.Models.DTO
{
	public class CityModel
	{
		public int Id { get; set; }

		public string? Name { get; set; }

		public RegionModel? Region { get; set; }
	}
}
