namespace Marketplace.Dto
{
	public class PageDto
	{
		public PageDto(IEnumerable<ItemDto> items, int totalItems)
		{
			Items = items;
			TotalItems = totalItems;
		}

		public IEnumerable<ItemDto> Items { get; set; }
		public int TotalItems { get; set; }
	}
}
