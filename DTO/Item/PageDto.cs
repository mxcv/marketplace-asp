namespace Marketplace.Dto
{
	public class PageDto
	{
		public PageDto(IEnumerable<ItemDto> items, int leftCount)
		{
			Items = items;
			LeftCount = leftCount;
		}

		public IEnumerable<ItemDto> Items { get; set; }
		public int LeftCount { get; set; }
	}
}
