using System.ComponentModel.DataAnnotations;
using Marketplace.Dto;
using Marketplace.Models;

namespace Marketplace.ViewModels
{
	public class IndexViewModel
	{
		public IndexViewModel(PaginatedList<ItemDto> items, FilterViewModel filter, SortType? sortType)
		{
			Items = items;
			Filter = filter;
			SortType = sortType;
		}

		public PaginatedList<ItemDto> Items { get; set; }

		public FilterViewModel Filter { get; set; }

		[Display(Name = "SortType")]
		public SortType? SortType { get; set; }
	}
}
