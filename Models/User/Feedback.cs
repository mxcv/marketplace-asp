namespace Marketplace.Models
{
	public class Feedback
	{
		public int Id { get; set; }
		public int ReviewerId { get; set; }
		public int SellerId { get; set; }

		public int Rate { get; set; }
		public string? Text { get; set; }
		public DateTimeOffset Created { get; set; }

		public virtual User Reviewer { get; set; } = null!;
		public virtual User Seller { get; set; } = null!;
	}
}
