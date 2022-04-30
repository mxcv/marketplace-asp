using Microsoft.EntityFrameworkCore;

namespace Marketplace.Models
{
	public class PaginatedList<T> : List<T>
	{
		public int PageIndex { get; private set; }
		public int PageSize { get; private set; }
		public int TotalPages { get; private set; }
		public int TotalCount { get; private set; }

		public PaginatedList()
		{
		}

		public PaginatedList(List<T> items, int pageIndex, int pageSize, int count)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
			TotalCount = count;

			AddRange(items);
		}

		public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
		{
			int count = await source.CountAsync();
			if (count == 0)
				return new PaginatedList<T>();
			if (pageIndex < 1)
				pageIndex = 1;
			if (pageSize < 1)
				pageSize = count;

			var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
			return new PaginatedList<T>(items, pageIndex, pageSize, count);
		}
	}
}
