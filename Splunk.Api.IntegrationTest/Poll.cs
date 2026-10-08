namespace Splunk.Api.IntegrationTest;

/// <summary>
/// Bounded waits for the shared instance's eventual consistency: a sidecar restarting, a token or catalog being built in
/// the background, a connection reset during the TLS handshake. The last attempt's result or exception is the answer.
/// </summary>
internal static class Poll
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	/// <summary>Reads until <paramref name="done"/> accepts the value, at most <paramref name="attempts"/> times.</summary>
	public static async Task<T> UntilAsync<T>(Func<Task<T>> read, Func<T, bool> done, int attempts, TimeSpan delay)
	{
		for (var attempt = 1; attempt < attempts; attempt++)
		{
			var value = await read();
			if (done(value))
			{
				return value;
			}

			await Task.Delay(delay, Ct);
		}

		return await read();
	}

	/// <summary>Runs <paramref name="action"/> again while it throws a transient <typeparamref name="TException"/>, at most <paramref name="attempts"/> times.</summary>
	public static async Task<T> RetryAsync<T, TException>(Func<Task<T>> action, Func<TException, bool> transient, int attempts, TimeSpan delay)
		where TException : Exception
	{
		for (var attempt = 1; attempt < attempts; attempt++)
		{
			try
			{
				return await action();
			}
			catch (TException exception) when (transient(exception))
			{
				await Task.Delay(delay, Ct);
			}
		}

		return await action();
	}

	/// <summary>Runs <paramref name="action"/> again while it throws a transient <typeparamref name="TException"/>, at most <paramref name="attempts"/> times.</summary>
	public static Task RetryAsync<TException>(Func<Task> action, Func<TException, bool> transient, int attempts, TimeSpan delay)
		where TException : Exception
		=> RetryAsync<bool, TException>(
			async () =>
			{
				await action();
				return true;
			},
			transient,
			attempts,
			delay);
}
