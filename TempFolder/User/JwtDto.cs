using System.ComponentModel.DataAnnotations;

namespace Marketplace.Dto
{
	public class JwtDto
	{
		[Required]
		public string AccessToken { get; set; } = null!;

		[Required]
		public string RefreshToken { get; set; } = null!;
	}
}
