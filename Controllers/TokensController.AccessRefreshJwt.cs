using System.ComponentModel.DataAnnotations;

namespace Marketplace.Controllers
{
	public partial class TokensController
	{
		public class AccessRefreshJwt
		{
			[Required]
			public string AccessToken { get; set; } = null!;

			[Required]
			public string RefreshToken { get; set; } = null!;
		}
	}
}
