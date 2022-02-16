﻿using System.ComponentModel.DataAnnotations;

namespace Marketplace.Controllers
{
	public partial class UsersController
	{
		public class UserViewModel
		{
			[Required]
			public string Email { get; set; } = null!;

			[Required]
			public string Password { get; set; } = null!;

			public string? PhoneNumber { get; set; }

			public string? Name { get; set; }
		}
	}
}
