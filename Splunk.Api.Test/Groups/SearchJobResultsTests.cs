using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SearchJobResultsTests
{
	private const string ResultsPath = "/services/search/v2/jobs/my_sid/results";
	private const string PreviewPath = "/services/search/v2/jobs/my_sid/results_preview";
	private const string EventsPath = "/services/search/v2/jobs/my_sid/events";

	private static readonly SearchResultsOptions PageOptions = new() { Count = 2, Offset = 4, Fields = ["host", "x"], AddSummaryToMetadata = true };
	private static readonly SearchResultsRequest PostProcess = new() { Search = "stats count by host", Count = 2, Offset = 4, Fields = ["host"], AddSummaryToMetadata = false };

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task GetResultsAsync_SendsGetWithPaging()
	{
		var stub = TestClient.Stub(SearchJson.Results);
		using var client = TestClient.Create(stub);

		await client.SearchJobResults.GetResultsAsync("my_sid", PageOptions, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ResultsPath, "?add_summary_to_metadata=true&count=2&offset=4&f=host&f=x&output_mode=json");
	}

	[Fact]
	public async Task GetResultsAsync_MapsTheResults()
	{
		using var client = TestClient.Create(TestClient.Stub(SearchJson.Results));

		var results = await client.SearchJobResults.GetResultsAsync("my_sid", null, Ct);

		SearchResultsAssert.AssertResults(results);
	}

	[Fact]
	public async Task GetResultsAsync_MapsAnEmptyPage()
	{
		using var client = TestClient.Create(TestClient.Stub(SearchJson.EmptyResults));

		var results = await client.SearchJobResults.GetResultsAsync("my_sid", null, Ct);

		results.Results.Should().BeEmpty();
		results.Fields.Should().BeEmpty();
		results.Highlighted.Should().BeEmpty();
		results.PostProcessCount.Should().Be(0);
	}

	[Fact]
	public async Task GetResultsCsvAsync_SendsGetForCsvAndStreamsIt()
	{
		var stub = SearchRequestAssert.TextStub("host\nsplunk01\n", "text/csv");
		using var client = TestClient.Create(stub);

		using var reader = new StreamReader(await client.SearchJobResults.GetResultsCsvAsync("my_sid", new SearchResultsOptions { Count = 2 }, Ct));

		SearchRequestAssert.Sent(stub, HttpMethod.Get, ResultsPath, "?output_mode=csv&count=2");
		(await reader.ReadToEndAsync(Ct)).Should().Be("host\nsplunk01\n");
	}

	[Fact]
	public async Task PostProcessResultsAsync_SendsTheSearchAsForm()
	{
		var stub = TestClient.Stub(SearchJson.Results);
		using var client = TestClient.Create(stub);

		var results = await client.SearchJobResults.PostProcessResultsAsync("my_sid", PostProcess, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, ResultsPath, "?output_mode=json", "search=stats+count+by+host&count=2&offset=4&f=host&add_summary_to_metadata=false");
		results.PostProcessCount.Should().Be(3);
	}

	[Fact]
	public async Task GetPreviewAsync_SendsGetWithPaging()
	{
		var stub = TestClient.Stub(SearchJson.Results);
		using var client = TestClient.Create(stub);

		var results = await client.SearchJobResults.GetPreviewAsync("my_sid", PageOptions, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, PreviewPath, "?add_summary_to_metadata=true&count=2&offset=4&f=host&f=x&output_mode=json");
		results.Results.Should().HaveCount(2);
	}

	[Fact]
	public async Task PostProcessPreviewAsync_SendsTheSearchAsForm()
	{
		var stub = TestClient.Stub(SearchJson.Results);
		using var client = TestClient.Create(stub);

		await client.SearchJobResults.PostProcessPreviewAsync("my_sid", PostProcess, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, PreviewPath, "?output_mode=json", "search=stats+count+by+host&count=2&offset=4&f=host&add_summary_to_metadata=false");
	}

	[Fact]
	public async Task GetEventsAsync_SendsGetWithEveryOption()
	{
		var stub = TestClient.Stub(SearchJson.Results);
		using var client = TestClient.Create(stub);

		var events = await client.SearchJobResults.GetEventsAsync(
			"my_sid",
			new SearchEventsOptions
			{
				Count = 1,
				Offset = -1,
				Fields = ["_raw"],
				EarliestTime = "-1h",
				LatestTime = "now",
				MaxLines = 5,
				TruncationMode = TruncationMode.Truncate,
				Segmentation = "full",
				OutputTimeFormat = "%s",
				TimeFormat = "%s"
			},
			Ct);

		SearchRequestAssert.Sent(stub,
			HttpMethod.Get,
			EventsPath,
			"?earliest_time=-1h&latest_time=now&max_lines=5&truncation_mode=truncate&segmentation=full&output_time_format=%25s"
				+ "&time_format=%25s&count=1&offset=-1&f=_raw&output_mode=json");
		events.Results.Should().HaveCount(2);
	}

	[Fact]
	public async Task GetEventsRawAsync_SendsGetForRawText()
	{
		var stub = SearchRequestAssert.TextStub("10-08-2026 13:47:49.150 +0000 INFO  Metrics - group=metadata_metrics\n", "text/plain");
		using var client = TestClient.Create(stub);

		using var reader = new StreamReader(await client.SearchJobResults.GetEventsRawAsync("my_sid", new SearchEventsOptions { TruncationMode = TruncationMode.Abstract }, Ct));

		SearchRequestAssert.Sent(stub, HttpMethod.Get, EventsPath, "?output_mode=raw&truncation_mode=abstract");
		(await reader.ReadLineAsync(Ct)).Should().Contain("group=metadata_metrics");
	}

	[Fact]
	public async Task PostProcessEventsAsync_SendsEveryParameterAsForm()
	{
		var stub = TestClient.Stub(SearchJson.Results);
		using var client = TestClient.Create(stub);

		await client.SearchJobResults.PostProcessEventsAsync(
			"my_sid",
			new SearchEventsRequest
			{
				Search = "search host=splunk01",
				Count = 1,
				Offset = 0,
				Fields = ["_raw", "host"],
				EarliestTime = "-1h",
				LatestTime = "now",
				MaxLines = 1,
				TruncationMode = TruncationMode.Abstract,
				Segmentation = "raw",
				OutputTimeFormat = "%s",
				TimeFormat = "%s"
			},
			Ct);

		SearchRequestAssert.Sent(stub,
			HttpMethod.Post,
			EventsPath,
			"?output_mode=json",
			"earliest_time=-1h&latest_time=now&max_lines=1&truncation_mode=abstract&segmentation=raw&output_time_format=%25s"
				+ "&time_format=%25s&search=search+host%3Dsplunk01&count=1&offset=0&f=_raw&f=host");
	}

	[Fact]
	public async Task GetResultsCsvAsync_Error_RaisesSplunkApiException()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.NotFound, """<?xml version="1.0"?><response><messages><msg type="FATAL">Unknown sid.</msg></messages></response>""");
		using var client = TestClient.Create(stub);

		await SearchRequestAssert.FailsWith(() => client.SearchJobResults.GetResultsCsvAsync("gone", null, Ct), HttpStatusCode.NotFound, "Unknown sid.");
	}
}
