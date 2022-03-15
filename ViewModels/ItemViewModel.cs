using System.ComponentModel.DataAnnotations;
using Marketplace.Dto;

namespace Marketplace.ViewModels
{
	public class ItemViewModel
	{
		[Required]
		[StringLength(50)]
		public string Title { get; set; } = null!;

		[StringLength(1000)]
		public string? Description { get; set; }

		[Range(0, double.PositiveInfinity)]
		public decimal? Price { get; set; }

		public CurrencyDto? Currency { get; set; }

		public CategoryDto? Category { get; set; }
	}
}
