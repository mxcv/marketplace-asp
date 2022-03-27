﻿using System.ComponentModel.DataAnnotations;

namespace Marketplace.ViewModels
{
	public class WebRegisterViewModel : RegisterViewModel
	{
		[Required(ErrorMessage = "ConfirmPasswordRequired")]
		[DataType(DataType.Password)]
		[StringLength(30)]
		[Compare(nameof(Password), ErrorMessage = "ConfirmPasswordCompare")]
		[Display(Name = "ConfirmPassword")]
		public string ConfirmPassword { get; set; } = null!;
	}
}
