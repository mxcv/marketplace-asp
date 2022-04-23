namespace Marketplace.Dto
{
	public class FeedbackStatisticsDto
	{
		public FeedbackStatisticsDto(int count, double average)
		{
			Count = count;
			Average = average;
		}

		public int Count { get; set; }
		public double Average { get; set; }
	}
}
