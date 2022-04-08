namespace Marketplace.ViewModels
{
	public class PageViewModel
	{
		public PageViewModel(int? index, int? size)
		{
			if (index == null || index < 1)
				index = 1;
			if (size == null || size < 1)
				size = 20;
			else if (size > 100)
				size = 100;

			Index = index.Value;
			Size = size.Value;
		}

		public int Index { get; private set; }
		public int Size { get; private set; }
		public int TotalPages { get; set; }
		public int TotalItems { get; set; }
	}
}
