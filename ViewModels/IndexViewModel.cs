using Marketplace.Dto;
using Marketplace.Models;

namespace Marketplace.ViewModels
{
	public class IndexViewModel
	{
		public IndexViewModel()
		{
		}

		public IndexViewModel(SortType? sortType, FilterViewModel? filter, PageViewModel? page)
		{
			SortType = sortType;
			Filter = filter;
			Page = page;
		}

		public IEnumerable<ItemDto>? Items { get; set; }
		public SortType? SortType { get; set; }
		public FilterViewModel? Filter { get; set; }
		public PageViewModel? Page { get; set; }
	}
}
