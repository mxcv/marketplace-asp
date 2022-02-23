namespace Marketplace.Models
{
	public class Category
	{
		public int Id { get; set; }

		public virtual ICollection<CategoryTitle> Titles { get; set; } = null!;
		public virtual ICollection<Item> Items { get; set; } = null!;
	}
}
