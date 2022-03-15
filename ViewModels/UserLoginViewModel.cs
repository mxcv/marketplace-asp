using System.ComponentModel.DataAnnotations;

namespace Marketplace.ViewModels
{
	public class UserLoginViewModel
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
	}
}
