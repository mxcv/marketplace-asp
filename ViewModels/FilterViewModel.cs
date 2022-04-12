using System.ComponentModel.DataAnnotations;

namespace Marketplace.ViewModels
{
	public class FilterViewModel
	{
		[StringLength(100)]
		[Display(Name = "Query")]
		public string? Query { get; set; }

		[Range(0, double.PositiveInfinity)]
		[Display(Name = "MinPrice")]
		public decimal? MinPrice { get; set; }

		[Range(0, double.PositiveInfinity)]
		[Display(Name = "MaxPrice")]
		public decimal? MaxPrice { get; set; }

		[Display(Name = "Category")]
		public int? CategoryId { get; set; }

		[Display(Name = "Currency")]
		public int? CurrencyId { get; set; }

		[Display(Name = "Country")]
		public int? CountryId { get; set; }

		[Display(Name = "Region")]
		public int? RegionId { get; set; }

		[Display(Name = "City")]
		public int? CityId { get; set; }

		public int? UserId { get; set; }
	}
}
