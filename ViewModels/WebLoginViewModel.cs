using System.ComponentModel.DataAnnotations;

namespace Marketplace.ViewModels
{
	public class WebLoginViewModel : LoginViewModel
	{
		[Display(Name = "RememberMe")]
		public bool RememberMe { get; set; }

		public string? ReturnUrl { get; set; }
	}
}
