namespace Marketplace.Models
{
	public class Item
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int? CategoryId { get; set; }

		public string Title { get; set; } = null!;
		public string? Description { get; set; }
		public DateTimeOffset Created { get; set; }

		public virtual Price? Price { get; set; }
		public virtual Category? Category { get; set; }
		public virtual User User { get; set; } = null!;
		public virtual ICollection<ItemImage> Images { get; set; } = null!;
	}
}
