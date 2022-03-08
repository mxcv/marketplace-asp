namespace Marketplace.Models
{
	public class MediaFile
	{
		public int Id { get; set; }

		public string Extension { get; set; } = null!;

		public virtual ICollection<ItemImage> ItemImages { get; set; } = null!;
		public virtual ICollection<UserImage> UserImages { get; set; } = null!;

		public string Name => Id + Extension;
	}
}
