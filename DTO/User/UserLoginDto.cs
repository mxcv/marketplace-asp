﻿using System.ComponentModel.DataAnnotations;

namespace Marketplace.DTO
{
	public class UserLoginDto
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;
	}
}
