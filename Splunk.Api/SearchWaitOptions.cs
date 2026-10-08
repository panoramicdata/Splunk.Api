namespace Splunk.Api;

/// <summary>How <see cref="SplunkSearch"/> waits for a search job to finish.</summary>
public class SearchWaitOptions
{
	/// <summary>How long to wait between polls of the job's state. The default is half a second.</summary>
	public TimeSpan PollInterval { get; init; } = TimeSpan.FromMilliseconds(500);

	/// <summary>
	/// How long to wait for the job to finish before giving up with a <see cref="TimeoutException"/>, or
	/// <see langword="null"/> (the default) to wait until it finishes or the operation is cancelled.
	/// </summary>
	public TimeSpan? Timeout { get; init; }
}
