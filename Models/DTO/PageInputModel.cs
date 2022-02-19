using System.ComponentModel.DataAnnotations;

namespace Marketplace.Models.DTO
{
	public class PageInputModel
	{
		[Required]
		[Range(0, int.MaxValue)]
		public int SkipCount { get; set; }

		[Required]
		[Range(0, int.MaxValue)]
		public int TakeCount { get; set; }
	}
}
