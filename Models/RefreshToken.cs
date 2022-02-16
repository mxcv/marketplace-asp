namespace Marketplace.Models
{
	public class RefreshToken
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string Token { get; set; } = null!;
		public DateTime? Expired { get; set; }

		public virtual User User { get; set; } = null!;
	}
}
