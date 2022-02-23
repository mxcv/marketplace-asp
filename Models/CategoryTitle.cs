namespace Marketplace.Models
{
	public class CategoryTitle
	{
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public int LanguageId { get; set; }

		public string Value { get; set; } = null!;

		public virtual Category Category { get; set; } = null!;
		public virtual Language Language { get; set; } = null!;
	}
}
