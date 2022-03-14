namespace Marketplace.DTO
{
	public class UserDto
	{
		public int Id { get; set; }

		public string Email { get; set; } = null!;
		public string? PhoneNumber { get; set; }
		public string? Name { get; set; }
		public DateTimeOffset Created { get; set; }

		public CityDto? City { get; set; }
		public ImageDto? Image { get; set; }
	}
}
