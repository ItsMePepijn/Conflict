using Microsoft.AspNetCore.SignalR.Client;

public class RetryPolicy : IRetryPolicy
{
	public long RetryCount { get; set; }

	public TimeSpan? NextRetryDelay(RetryContext retryContext)
	{
		if (retryContext.ElapsedTime < TimeSpan.FromSeconds(10))
		{
			return TimeSpan.FromSeconds(1);
		}
		else if (retryContext.ElapsedTime < TimeSpan.FromSeconds(60))
		{
			return TimeSpan.FromSeconds(5);
		}
		else
		{
			return null;
		}
	}
}