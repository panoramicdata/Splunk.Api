using Splunk.Api.Models;
using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public partial class SearchJobsTests
{
	private static readonly string JobFeed = SearchRequestAssert.Feed("search index=_internal | head 5", SearchJson.JobContent);

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ListAsync_SendsGetWithPagingAndFilter()
	{
		var stub = TestClient.Stub(JobFeed);
		using var client = TestClient.Create(stub);

		await client.SearchJobs.ListAsync(new ListOptions { Count = 2, Search = "isDone=1" }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/search/jobs", "?count=2&search=isDone%3D1&output_mode=json");
	}

	[Fact]
	public async Task GetAsync_SendsGetToTheEscapedSid()
	{
		var stub = TestClient.Stub(JobFeed);
		using var client = TestClient.Create(stub);

		await client.SearchJobs.GetAsync("scheduler__admin__search__RMD5_at_1791467606_3", Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/search/jobs/scheduler__admin__search__RMD5_at_1791467606_3", "?output_mode=json");
	}

	[Fact]
	public async Task GetAsync_MapsEveryModelledJobField()
	{
		using var client = TestClient.Create(TestClient.Stub(JobFeed));

		var feed = await client.SearchJobs.GetAsync("splunk_api_probe_2", Ct);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("search index=_internal | head 5");
		AssertJob(entry.Content!);
	}

	private static void AssertJob(SearchJob job)
	{
		job.Sid.Should().Be("splunk_api_probe_2");
		job.DispatchState.Should().Be(SearchDispatchState.Done);
		job.IsDone.Should().BeTrue();
		job.IsFailed.Should().BeFalse();
		job.IsFinalized.Should().BeFalse();
		job.IsPaused.Should().BeFalse();
		job.IsSaved.Should().BeFalse();
		job.IsSavedSearch.Should().BeFalse();
		job.IsRealTimeSearch.Should().BeFalse();
		job.IsZombie.Should().BeFalse();
		job.IsPreviewEnabled.Should().BeTrue();
		job.DoneProgress.Should().Be(1);
		job.EventCount.Should().Be(5);
		job.EventAvailableCount.Should().Be(5);
		job.EventFieldCount.Should().Be(15);
		job.EventIsStreaming.Should().BeTrue();
		job.EventIsTruncated.Should().BeFalse();
		job.EventSearch.Should().Be("search index=_internal ");
		job.EventSorting.Should().Be("desc");
		job.ResultCount.Should().Be(5);
		job.ResultPreviewCount.Should().Be(5);
		job.ResultIsStreaming.Should().BeFalse();
		job.ScanCount.Should().Be(12);
		job.DropCount.Should().Be(0);
		job.DiskUsage.Should().Be(65536);
		job.RunDuration.Should().Be(0.014);
		job.Priority.Should().Be(5);
		job.Ttl.Should().Be(600);
		job.DefaultTtl.Should().Be(600);
		job.DefaultSaveTtl.Should().Be(604800);
		job.StatusBuckets.Should().Be(300);
		job.NumPreviews.Should().Be(2);
		AssertJobDetails(job);
	}

	private static void AssertJobDetails(SearchJob job)
	{
		job.EarliestTime.Should().Be(new DateTimeOffset(2026, 10, 8, 12, 47, 15, TimeSpan.Zero));
		job.LatestTime.Should().Be(new DateTimeOffset(2026, 10, 8, 13, 47, 15, TimeSpan.Zero));
		job.CursorTime.Should().Be(DateTimeOffset.UnixEpoch);
		job.SearchEarliestTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1791463635));
		job.SearchLatestTime.Should().Be(DateTimeOffset.FromUnixTimeMilliseconds(1791467235500));
		job.Search.Should().Be("search index=_internal | head 5");
		job.OptimizedSearch.Should().Be("| search index=_internal | head 5");
		job.RemoteSearch.Should().Be("litsearch index=_internal | fields keepcolorder=t \"_raw\"");
		job.ReportSearch.Should().Be("head 5");
		job.Keywords.Should().Be("index::_internal");
		job.Label.Should().BeEmpty();
		job.Delegate.Should().BeEmpty();
		job.Provenance.Should().Be("rest:jobs");
		job.WorkloadPool.Should().BeEmpty();
		job.Messages.Should().ContainSingle().Which.ToString().Should().Be("INFO: Your timerange was substituted.");
		job.Request.Should().Contain("status_buckets", "300").And.Contain("exec_mode", "blocking");
		job.Runtime.Should().Contain("auto_cancel", "0");
		job.SearchProviders.Should().Equal("splunk01");
		job.AdditionalProperties.Should().ContainKeys("performance", "pid", "canSummarize");
	}

	[Fact]
	public async Task CreateAsync_SendsEveryTypedParameterAsForm()
	{
		var stub = TestClient.Stub("""{"sid":"1791467235.42"}""");
		using var client = TestClient.Create(stub);

		await client.SearchJobs.CreateAsync(
			new SearchJobCreateRequest
			{
				Search = "search index=_internal | head 5",
				EarliestTime = "-1h@h",
				LatestTime = "now",
				Now = "1791467235",
				IndexEarliest = "-2h",
				IndexLatest = "-1m",
				TimeFormat = "%s",
				SearchMode = SearchMode.Normal,
				AdhocSearchLevel = AdhocSearchLevel.Smart,
				RequiredFields = ["host", "source"],
				Namespace = "search",
				EnableLookups = false,
				ReloadMacros = true,
				AllowPartialResults = false,
				WorkloadPool = "standard_perf",
				MaxCount = 500,
				MaxTime = 60,
				StatusBuckets = 300,
				Timeout = 120,
				AutoCancel = 30,
				AutoFinalizeEventCount = 1000,
				AutoPause = 40,
				ReduceFrequency = 10,
				ReuseMaxSecondsAgo = 5,
				ExecutionMode = SearchExecutionMode.Blocking,
				Id = "my_sid",
				AdditionalParameters = { ["custom.ticket"] = "INC-1" }
			},
			Ct);

		SearchRequestAssert.Sent(stub,
			HttpMethod.Post,
			"/services/search/jobs",
			"?output_mode=json",
			"exec_mode=blocking&id=my_sid&search=search+index%3D_internal+%7C+head+5&earliest_time=-1h%40h&latest_time=now"
				+ "&now=1791467235&index_earliest=-2h&index_latest=-1m&time_format=%25s&search_mode=normal&adhoc_search_level=smart"
				+ "&rf=host&rf=source&namespace=search&enable_lookups=false&reload_macros=true&allow_partial_results=false"
				+ "&workload_pool=standard_perf&max_count=500&max_time=60&status_buckets=300&timeout=120&auto_cancel=30"
				+ "&auto_finalize_ec=1000&auto_pause=40&reduce_freq=10&reuse_max_seconds_ago=5&custom.ticket=INC-1");
	}

	[Fact]
	public async Task CreateAsync_ReturnsTheSid()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"sid":"1791467235.42"}"""));

		var created = await client.SearchJobs.CreateAsync(new SearchJobCreateRequest { Search = "| makeresults" }, Ct);

		created.Sid.Should().Be("1791467235.42");
	}

	[Fact]
	public async Task RunOneshotAsync_SendsOneshotForm()
	{
		var stub = TestClient.Stub(SearchJson.Results);
		using var client = TestClient.Create(stub);

		await client.SearchJobs.RunOneshotAsync(
			new SearchOneshotRequest { Search = "| makeresults count=2", Count = 0, Offset = 1, Fields = ["_time", "x"], EarliestTime = "-1d" },
			Ct);

		SearchRequestAssert.Sent(stub,
			HttpMethod.Post,
			"/services/search/jobs",
			"?output_mode=json",
			"search=%7C+makeresults+count%3D2&earliest_time=-1d&exec_mode=oneshot&count=0&offset=1&f=_time&f=x");
	}

	[Fact]
	public async Task RunOneshotAsync_MapsTheResults()
	{
		using var client = TestClient.Create(TestClient.Stub(SearchJson.Results));

		var results = await client.SearchJobs.RunOneshotAsync(new SearchOneshotRequest { Search = "| makeresults" }, Ct);

		SearchResultsAssert.AssertResults(results);
	}

	[Fact]
	public async Task CreateAsync_Error_RaisesSplunkApiException()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"FATAL","text":"Unknown search command 'nosuchcommand'."}]}""", HttpStatusCode.BadRequest));

		await SearchRequestAssert.FailsWith(() => client.SearchJobs.CreateAsync(new SearchJobCreateRequest { Search = "| nosuchcommand" }, Ct), HttpStatusCode.BadRequest, "Unknown search command 'nosuchcommand'.");
	}
}
