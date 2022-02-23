namespace Marketplace.Models
{
	public class CityName
	{
		public int Id { get; set; }
		public int CityId { get; set; }
		public int LanguageId { get; set; }

		public string Value { get; set; } = null!;

		public virtual City City { get; set; } = null!;
		public virtual Language Language { get; set; } = null!;
	}
}
