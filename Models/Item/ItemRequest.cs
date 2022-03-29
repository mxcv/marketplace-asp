namespace Marketplace.Models
{
	public class ItemRequest
	{
		public string? Query { get; set; }
		public decimal? MinPrice { get; set; }
		public decimal? MaxPrice { get; set; }
		public int? CategoryId { get; set; }
		public int? CountryId { get; set; }
		public int? RegionId { get; set; }
		public int? CityId { get; set; }
		public int? UserId { get; set; }
		public int? SortTypeId { get; set; }
		public int? SkipCount { get; set; }
		public int? TakeCount { get; set; }
	}
}
