using System.ComponentModel.DataAnnotations;

namespace Marketplace.ViewModels
{
	public class LoginViewModel : ApiLoginViewModel
	{
		[Display(Name = "RememberMe")]
		public bool RememberMe { get; set; }

		public string? ReturnUrl { get; set; }
	}
}
