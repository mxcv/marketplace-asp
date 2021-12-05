using System.ComponentModel.DataAnnotations;

namespace Marketplace.Controllers
{
	public partial class UsersController
	{
		public class UserViewModel
		{
			//[Required]
			//[EmailAddress]
			public string? Email { get; set; }

			//[Required]
			public string? Password { get; set; }

			public string? PhoneNumber { get; set; }

			public string? Name { get; set; }
		}
	}
}
