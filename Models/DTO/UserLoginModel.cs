using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models.DTO
{
	public class UserLoginModel
	{
		[Required]
		public string Email { get; set; } = null!;

		[Required]
		public string Password { get; set; } = null!;
	}
}
