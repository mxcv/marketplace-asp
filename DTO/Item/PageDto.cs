using Marketplace.ViewModels;

namespace Marketplace.Dto
{
	public class PageDto : PageViewModel
	{
		public PageDto(IEnumerable<ItemDto> items, PageViewModel pageModel)
			: base(pageModel.Index, pageModel.Size)
		{
			Items = items;
			TotalPages = pageModel.TotalPages;
			TotalItems = pageModel.TotalItems;
		}

		public IEnumerable<ItemDto> Items { get; set; }
	}
}
