using System.ComponentModel.DataAnnotations;
using Marketplace.Dto;

namespace Marketplace.ViewModels
{
	public class UserRegisterViewModel : UserLoginViewModel
	{
		[DataType(DataType.PhoneNumber)]
		public string? PhoneNumber { get; set; }

		[DataType(DataType.Text)]
		public string? Name { get; set; }

		public CityDto? City { get; set; }
	}
}
