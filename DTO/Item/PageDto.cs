namespace Marketplace.Dto
{
	public class PageDto
	{
		public PageDto(ICollection<ItemDto> items, int leftCount)
		{
			Items = items;
			LeftCount = leftCount;
		}

		public ICollection<ItemDto> Items { get; set; }
		public int LeftCount { get; set; }
	}
}
