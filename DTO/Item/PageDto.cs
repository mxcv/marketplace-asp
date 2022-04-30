using Marketplace.Models;

namespace Marketplace.Dto
{
	public class PageDto<T>
	{
		public PageDto(PaginatedList<T> list)
		{
			Items = list;
			PageIndex = list.PageIndex;
			PageSize = list.PageSize;
			TotalPages = list.TotalPages;
			TotalCount = list.TotalCount;
		}

		public IEnumerable<T> Items { get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public int TotalPages { get; set; }
		public int TotalCount { get; set; }
	}
}
