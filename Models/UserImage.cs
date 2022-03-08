namespace Marketplace.Models
{
	public class UserImage
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int FileId { get; set; }

		public virtual User User { get; set; } = null!;
		public virtual MediaFile File { get; set; } = null!;
	}
}
