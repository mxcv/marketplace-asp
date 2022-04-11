namespace Marketplace.ViewModels
{
	public class ItemViewModel : ApiItemViewModel
	{
		public IFormFileCollection? Images { get; set; }

		public int CurrencyId { get; set; }

		public int? CategoryId { get; set; }
	}
}
