using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models.DTO
{
	public class JwtModel
	{
		[Required]
		public string AccessToken { get; set; } = null!;

		[Required]
		public string RefreshToken { get; set; } = null!;
	}
}
