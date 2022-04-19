using Marketplace.Models;

namespace Marketplace.Dto
{
	public class PageDto
	{
		public PageDto(PaginatedList<ItemDto> list)
		{
			Items = list;
			TotalCount = list.TotalCount;
		}

		public IEnumerable<ItemDto> Items { get; set; }
		public int TotalCount { get; set; }
	}
}
