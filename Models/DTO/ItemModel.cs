using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models.DTO
{
	public class ItemModel
	{
		public int? Id { get; set; }

		[Required]
		[StringLength(50)]
		public string Title { get; set; } = null!;

		[StringLength(1000)]
		public string? Description { get; set; }

		public DateTime Created { get; set; }

		[Range(0, double.PositiveInfinity)]
		public decimal? Price { get; set; }

		public CurrencyModel? Currency { get; set; }

		public UserModel? User { get; set; }
	}
}
