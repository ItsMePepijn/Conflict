using Microsoft.AspNetCore.SignalR.Client;

public class RetryPolicy : IRetryPolicy
{
	public long RetryCount { get; set; }

	public TimeSpan? NextRetryDelay(RetryContext retryContext)
	{
		if (retryContext.ElapsedTime < TimeSpan.FromSeconds(10))
		{
			Console.WriteLine("Starting on reconnect " + (retryContext.PreviousRetryCount + 1));
			return TimeSpan.FromSeconds(1);
		}
		else if (retryContext.ElapsedTime < TimeSpan.FromSeconds(60))
		{
			Console.WriteLine("Starting on reconnect " + (retryContext.PreviousRetryCount + 1));
			return TimeSpan.FromSeconds(5);
		}
		else
		{
			Console.WriteLine("Failed to reconnect");
			return null;
		}
	}
}