using System.ComponentModel.DataAnnotations;

namespace Marketplace.DTO
{
	public class ItemDto
	{
		public int? Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Title { get; set; } = null!;

		[StringLength(1000)]
		public string? Description { get; set; }

		public DateTimeOffset Created { get; set; }

		[Range(0, double.PositiveInfinity)]
		public decimal? Price { get; set; }

		public CurrencyDto? Currency { get; set; }

		public CategoryDto? Category { get; set; }

		public UserDto? User { get; set; }

		public IEnumerable<ImageDto>? Images { get; set; }
	}
}
