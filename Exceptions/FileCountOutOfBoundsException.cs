namespace Marketplace.Exceptions
{
	public class FileCountOutOfBoundsException : Exception
	{
		public FileCountOutOfBoundsException()
		{
		}

		public FileCountOutOfBoundsException(string message)
			: base(message)
		{
		}

		public FileCountOutOfBoundsException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
