using System.ComponentModel.DataAnnotations;

namespace Marketplace.ViewModels
{
	public class WebLoginViewModel : UserLoginViewModel
	{
		[Display(Name = "Remeber me")]
		public bool RememberMe { get; set; }

		public string? ReturnUrl { get; set; }
	}
}
