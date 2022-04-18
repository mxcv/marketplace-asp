namespace Marketplace.Dto
{
	public class FeedbackDto
	{
		public int Id { get; set; }

		public int Rate { get; set; }
		public string? Text { get; set; }
		public DateTimeOffset Created { get; set; }

		public UserDto Reviewer { get; set; } = null!;
	}
}
