using System.ComponentModel.DataAnnotations;
using Marketplace.Dto;

namespace Marketplace.ViewModels
{
	public class RegisterViewModel : LoginViewModel
	{
		[DataType(DataType.PhoneNumber)]
		[StringLength(20)]
		[Display(Name = "PhoneNumber")]
		public string? PhoneNumber { get; set; }

		[DataType(DataType.Text)]
		[StringLength(20)]
		[Display(Name = "Name")]
		public string? Name { get; set; }

		public CityDto? City { get; set; }
	}
}
