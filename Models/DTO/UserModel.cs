using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models.DTO
{
	public class UserModel
	{
		[Required]
		public string Email { get; set; } = null!;

		[Required]
		public string Password { get; set; } = null!;

		public string? PhoneNumber { get; set; }

		public string? Name { get; set; }

		public DateTimeOffset Created { get; set; }

		public CityModel? City { get; set; }

		public ImageModel? Image { get; set; }
	}
}
