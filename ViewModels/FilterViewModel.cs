namespace Marketplace.ViewModels
{
	public class FilterViewModel
	{
		public string? Query { get; set; }
		public decimal? MinPrice { get; set; }
		public decimal? MaxPrice { get; set; }
		public int? CategoryId { get; set; }
		public int? CurrencyId { get; set; }
		public int? CountryId { get; set; }
		public int? RegionId { get; set; }
		public int? CityId { get; set; }
		public int? UserId { get; set; }
	}
}
