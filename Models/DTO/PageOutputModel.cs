namespace Marketplace.Models.DTO
{
	public class PageOutputModel
	{
		public PageOutputModel(ICollection<ItemModel> items, int leftCount)
		{
			Items = items;
			LeftCount = leftCount;
		}

		public ICollection<ItemModel> Items { get; set; }
		public int LeftCount { get; set; }
	}
}
