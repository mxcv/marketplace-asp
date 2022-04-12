using System.ComponentModel.DataAnnotations;

namespace Marketplace.ViewModels
{
	public class ItemViewModel : ApiItemViewModel
	{
		public IFormFileCollection? Images { get; set; }

		[Display(Name = "Currency")]
		public int CurrencyId { get; set; }

		[Display(Name = "Category")]
		public int? CategoryId { get; set; }
	}
}
