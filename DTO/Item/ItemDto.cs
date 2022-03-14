namespace Marketplace.Dto
{
	public class ItemDto
	{
		public int Id { get; set; }

		public string Title { get; set; } = null!;
		public string? Description { get; set; }
		public DateTimeOffset Created { get; set; }
		public decimal? Price { get; set; }

		public CurrencyDto? Currency { get; set; }
		public CategoryDto? Category { get; set; }
		public UserDto User { get; set; } = null!;
		public IEnumerable<ImageDto>? Images { get; set; }
	}
}
