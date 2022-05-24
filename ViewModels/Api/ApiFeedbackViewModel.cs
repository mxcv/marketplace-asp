using System.ComponentModel.DataAnnotations;
using Marketplace.Dto;

namespace Marketplace.ViewModels
{
	public class ApiFeedbackViewModel
	{
		[Range(1, 5)]
		public int Rate { get; set; }

		[StringLength(1000)]
		public string? Text { get; set; }

		public UserDto Seller { get; set; } = null!;
	}
}
