using System.ComponentModel.DataAnnotations;

namespace Marketplace.ViewModels
{
	public class UserLoginViewModel
	{
		[Required(ErrorMessage = "EmailRequired")]
		[DataType(DataType.EmailAddress)]
		[StringLength(254)]
		[Display(Name = "Email")]
		public string Email { get; set; } = null!;

		[Required(ErrorMessage = "PasswordRequired")]
		[DataType(DataType.Password)]
		[StringLength(30)]
		[Display(Name = "Password")]
		public string Password { get; set; } = null!;
	}
}
