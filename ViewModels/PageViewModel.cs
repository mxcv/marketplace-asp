namespace Marketplace.ViewModels
{
	public class PageViewModel
	{
		public PageViewModel(int pageIndex, int totalPages, int totalCount)
		{
			PageIndex = pageIndex;
			TotalPages = totalPages;
			TotalCount = totalCount;
		}

		public int PageIndex { get; set; }
		public int TotalPages { get; set; }
		public int TotalCount { get; set; }

		public bool HasPreviousPage => PageIndex > 1;
		public bool HasNextPage => PageIndex < TotalPages;
	}
}
