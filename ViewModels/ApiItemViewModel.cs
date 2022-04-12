using System.ComponentModel.DataAnnotations;
using Marketplace.Dto;

namespace Marketplace.ViewModels
{
	public class ApiItemViewModel
	{
		[Required]
		[StringLength(50)]
		[Display(Name = "Title")]
		public string Title { get; set; } = null!;

		[StringLength(1000)]
		[Display(Name = "Description")]
		public string? Description { get; set; }

		[Range(0, double.PositiveInfinity)]
		[Display(Name = "Price")]
		public decimal? Price { get; set; }

		public CurrencyDto? Currency { get; set; }

		public CategoryDto? Category { get; set; }
	}
}
