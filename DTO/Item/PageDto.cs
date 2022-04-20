using Marketplace.Models;

namespace Marketplace.Dto
{
	public class PageDto<T>
	{
		public PageDto(PaginatedList<T> list)
		{
			Items = list;
			TotalCount = list.TotalCount;
		}

		public IEnumerable<T> Items { get; set; }
		public int TotalCount { get; set; }
	}
}
