using System.ComponentModel.DataAnnotations;

namespace Marketplace.Dto
{
	public class UserLoginDto
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
	}
}
