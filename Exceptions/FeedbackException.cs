namespace Marketplace.Exceptions
{
	public class FeedbackException : Exception
	{
		public FeedbackException()
		{
		}

		public FeedbackException(string message)
			: base(message)
		{
		}

		public FeedbackException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
