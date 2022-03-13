namespace Marketplace.Models
{
	public class RegionName
	{
		public int Id { get; set; }
		public int RegionId { get; set; }
		public int LanguageId { get; set; }

		public string Value { get; set; } = null!;

		public virtual Region Region { get; set; } = null!;
		public virtual Language Language { get; set; } = null!;
	}
}
