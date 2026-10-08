using Splunk.Api.Interfaces;
using Splunk.Api.Models.Search;
using System.Runtime.CompilerServices;

namespace Splunk.Api;

/// <summary>
/// Runs searches end to end on top of <see cref="ISearchJobs"/>, <see cref="ISearchJobResults"/> and
/// <see cref="ISearchExport"/>: create a job, wait for it, page through its results and clean up; run a oneshot search;
/// or stream an export. Reach it through <see cref="SplunkClient.Search"/>.
/// </summary>
/// <example>
/// <code>
/// var run = await client.Search.RunAsync("search index=_internal | stats count by sourcetype", cancellationToken);
/// foreach (var row in run.Results)
/// {
///     Console.WriteLine($"{row["sourcetype"]}: {row["count"]}");
/// }
/// </code>
/// </example>
public sealed partial class SplunkSearch
{
	private readonly SplunkClient _client;

	internal SplunkSearch(SplunkClient client)
	{
		_client = client;
	}

	/// <summary>Waits between polls; replaceable so tests need not sleep.</summary>
	internal Func<TimeSpan, CancellationToken, Task> Delay { get; set; } = Task.Delay;

	/// <summary>Measures the wait timeout; replaceable so tests control time.</summary>
	internal TimeProvider TimeProvider { get; set; } = TimeProvider.System;

	/// <summary>Runs a search as a job and returns every result once it has finished. The job is then deleted.</summary>
	/// <param name="search">The search, for example <c>search index=_internal | head 10</c> or <c>| makeresults count=5</c>.</param>
	/// <param name="cancellationToken">Cancels the wait; the job is then cancelled and deleted.</param>
	/// <returns>The finished job and its results.</returns>
	/// <exception cref="SplunkSearchException">The search failed while running.</exception>
	public Task<SearchRunResult> RunAsync(string search, CancellationToken cancellationToken)
		=> RunAsync(new SearchJobCreateRequest { Search = search }, null, cancellationToken);

	/// <summary>
	/// Runs a search as a job: creates it, polls until it finishes, reads every result page by page and (unless
	/// <see cref="SearchRunOptions.DeleteJobWhenDone"/> is false) deletes it. A job that fails, times out or is
	/// cancelled is deleted.
	/// </summary>
	/// <param name="request">The search and its parameters.</param>
	/// <param name="options">Polling, timeout, paging and clean-up; <see langword="null"/> for the defaults.</param>
	/// <param name="cancellationToken">Cancels the operation; the job is then cancelled and deleted.</param>
	/// <returns>The finished job and its results.</returns>
	/// <exception cref="SplunkSearchException">The search failed while running.</exception>
	/// <exception cref="TimeoutException">The job did not finish within <see cref="SearchWaitOptions.Timeout"/>.</exception>
	public async Task<SearchRunResult> RunAsync(SearchJobCreateRequest request, SearchRunOptions? options, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request);
		options ??= new SearchRunOptions();
		var created = await _client.SearchJobs.CreateAsync(request, cancellationToken).ConfigureAwait(false);
		var keepJob = false;
		try
		{
			var job = await WaitForCompletionAsync(created.Sid, options, cancellationToken).ConfigureAwait(false);
			var pages = new List<SearchResults>();
			await foreach (var page in ReadPagesAsync(created.Sid, options.PageSize, cancellationToken).ConfigureAwait(false))
			{
				pages.Add(page);
			}

			keepJob = !options.DeleteJobWhenDone;
			return new SearchRunResult
			{
				Job = job,
				Fields = pages[0].Fields,
				Messages = pages[0].Messages,
				Results = [.. pages.SelectMany(p => p.Results)]
			};
		}
		finally
		{
			if (!keepJob)
			{
				await DeleteQuietlyAsync(created.Sid).ConfigureAwait(false);
			}
		}
	}

	/// <summary>Polls a job until it finishes.</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="options">Polling and timeout; <see langword="null"/> for the defaults.</param>
	/// <param name="cancellationToken">Cancels the wait (not the job).</param>
	/// <returns>The finished job.</returns>
	/// <exception cref="SplunkSearchException">The job failed, or its search process died.</exception>
	/// <exception cref="TimeoutException">The job did not finish within <see cref="SearchWaitOptions.Timeout"/>; it is left running.</exception>
	public async Task<SearchJob> WaitForCompletionAsync(string searchId, SearchWaitOptions? options, CancellationToken cancellationToken)
	{
		options ??= new SearchWaitOptions();
		var started = TimeProvider.GetTimestamp();
		while (true)
		{
			var feed = await _client.SearchJobs.GetAsync(searchId, cancellationToken).ConfigureAwait(false);
			var job = feed.Entries[0].Content!;
			if (HasFailed(job))
			{
				throw new SplunkSearchException(job.Messages, job);
			}

			if (job.IsDone)
			{
				return job;
			}

			if (HasTimedOut(options.Timeout, started))
			{
				throw new TimeoutException($"Search job '{searchId}' did not finish within {options.Timeout}.");
			}

			await Delay(options.PollInterval, cancellationToken).ConfigureAwait(false);
		}
	}

	/// <summary>Whether a job failed or its search process died.</summary>
	private static bool HasFailed(SearchJob job)
		=> job.IsFailed || job.IsZombie || job.DispatchState == SearchDispatchState.Failed;

	/// <summary>Whether a wait that began at <paramref name="started"/> has run past <paramref name="timeout"/>.</summary>
	private bool HasTimedOut(TimeSpan? timeout, long started)
		=> timeout is { } limit && TimeProvider.GetElapsedTime(started) >= limit;

	/// <summary>Reads every result of a finished job, a page at a time, as they are needed.</summary>
	/// <param name="searchId">The search ID.</param>
	/// <param name="pageSize">The results read per request.</param>
	/// <param name="cancellationToken">Stops reading.</param>
	/// <returns>The results, in order.</returns>
	public async IAsyncEnumerable<SearchResult> ReadResultsAsync(string searchId, int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		await foreach (var page in ReadPagesAsync(searchId, pageSize, cancellationToken).ConfigureAwait(false))
		{
			foreach (var result in page.Results)
			{
				yield return result;
			}
		}
	}

	private async IAsyncEnumerable<SearchResults> ReadPagesAsync(string searchId, int pageSize, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);
		var offset = 0;
		while (true)
		{
			var page = await _client.SearchJobResults
				.GetResultsAsync(searchId, new SearchResultsOptions { Count = pageSize, Offset = offset }, cancellationToken)
				.ConfigureAwait(false);
			yield return page;
			if (page.Results.Count < pageSize)
			{
				yield break;
			}

			offset += page.Results.Count;
		}
	}

	/// <summary>Deletes a job, ignoring failure: clean-up must not hide the error that led to it.</summary>
	private async Task DeleteQuietlyAsync(string searchId)
	{
		try
		{
			await _client.SearchJobs.DeleteAsync(searchId, CancellationToken.None).ConfigureAwait(false);
		}
		catch (Exception exception) when (exception is SplunkApiException or HttpRequestException or TimeoutException)
		{
			// The job expires on its own after its time to live.
		}
	}
}
