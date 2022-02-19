namespace Marketplace.Models
{
	public class Item
	{
		public int Id { get; set; }
		public int UserId { get; set; }

		public string Title { get; set; } = null!;
		public string? Description { get; set; }
		public DateTime Created { get; set; }

		public virtual Price? Price { get; set; }
		public virtual User User { get; set; } = null!;
	}
}
