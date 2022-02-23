namespace Marketplace.Models
{
	public class CountryName
	{
		public int Id { get; set; }
		public int CountryId { get; set; }
		public int LanguageId { get; set; }

		public string Value { get; set; } = null!;

		public virtual Country Country { get; set; } = null!;
		public virtual Language Language { get; set; } = null!;
	}
}
