using Marketplace.Dto;
using Marketplace.Models;

namespace Marketplace.ViewModels
{
	public class IndexViewModel
	{
		public IndexViewModel()
		{
			Filter = new FilterViewModel();
		}

		public IEnumerable<ItemDto>? Items { get; set; }
		public FilterViewModel Filter { get; set; }
		public SortType SortType { get; set; }
	}
}
