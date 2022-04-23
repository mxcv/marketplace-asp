namespace Marketplace.Dto
{
	public class UserDto
	{
		public int Id { get; set; }

		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Name { get; set; }
		public DateTimeOffset Created { get; set; }

		public FeedbackStatisticsDto? FeedbackStatistics { get; set; }
		public CityDto? City { get; set; }
		public ImageDto? Image { get; set; }
	}
}
