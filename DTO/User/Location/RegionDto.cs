namespace Marketplace.DTO
{
	public class RegionDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public IEnumerable<CityDto>? Cities { get; set; }
		public CountryDto? Country { get; set; }
	}
}
