using System.ComponentModel.DataAnnotations;

namespace Marketplace.DTO
{
	public class JwtDto
	{
		[Required]
		public string AccessToken { get; set; } = null!;

		[Required]
		public string RefreshToken { get; set; } = null!;
	}
}
