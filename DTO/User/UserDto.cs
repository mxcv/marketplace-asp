using System.ComponentModel.DataAnnotations;

namespace Marketplace.DTO
{
	public class UserDto
	{
		[Required]
		public string Email { get; set; } = null!;

		[Required]
		public string Password { get; set; } = null!;

		public string? PhoneNumber { get; set; }

		public string? Name { get; set; }

		public DateTimeOffset Created { get; set; }

		public CityDto? City { get; set; }

		public ImageDto? Image { get; set; }
	}
}
