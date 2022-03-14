using System.ComponentModel.DataAnnotations;

namespace Marketplace.DTO
{
	public class UserRegisterDto : UserLoginDto
	{
		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password))]
		public string ConfirmPassword { get; set; } = null!;

		[DataType(DataType.PhoneNumber)]
		public string? PhoneNumber { get; set; }

		[DataType(DataType.Text)]
		public string? Name { get; set; }

		public CityDto? City { get; set; }
	}
}
