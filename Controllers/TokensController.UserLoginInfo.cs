using System.ComponentModel.DataAnnotations;

namespace Marketplace.Controllers
{
	public partial class TokensController
	{
		public class UserLoginInfo
		{
			[Required]
			public string Email { get; set; } = null!;

			[Required]
			public string Password { get; set; } = null!;
		}
	}
}
