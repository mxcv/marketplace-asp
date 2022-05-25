namespace Marketplace.Dto
{
	public class CountryDto
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public IEnumerable<RegionDto>? Regions { get; set; }
	}
}
