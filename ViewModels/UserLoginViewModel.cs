using System.ComponentModel.DataAnnotations;

namespace Marketplace.ViewModels
{
	public class UserLoginViewModel
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		[StringLength(254)]
		public string Email { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		[StringLength(30)]
		public string Password { get; set; } = null!;
	}
}
