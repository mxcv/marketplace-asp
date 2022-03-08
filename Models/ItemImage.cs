namespace Marketplace.Models
{
	public class ItemImage
	{
		public int Id { get; set; }
		public int ItemId { get; set; }
		public int FileId { get; set; }

		public virtual Item Item { get; set; } = null!;
		public virtual MediaFile File { get; set; } = null!;
	}
}
