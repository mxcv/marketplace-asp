namespace Marketplace.Exceptions
{
	public class ItemCountOutOfBoundsException : Exception
	{
		public ItemCountOutOfBoundsException()
		{
		}

		public ItemCountOutOfBoundsException(string message)
			: base(message)
		{
		}

		public ItemCountOutOfBoundsException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
