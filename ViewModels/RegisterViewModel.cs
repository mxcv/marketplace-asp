﻿using System.ComponentModel.DataAnnotations;

namespace Marketplace.ViewModels
{
	public class RegisterViewModel : ApiRegisterViewModel
	{
		[Display(Name = "Avatar")]
		public IFormFile? Image { get; set; }

		[Required(ErrorMessage = "ConfirmPasswordRequired")]
		[DataType(DataType.Password)]
		[StringLength(30)]
		[Compare(nameof(Password), ErrorMessage = "ConfirmPasswordCompare")]
		[Display(Name = "ConfirmPassword")]
		public string ConfirmPassword { get; set; } = null!;

		public int? CityId { get; set; }
	}
}
