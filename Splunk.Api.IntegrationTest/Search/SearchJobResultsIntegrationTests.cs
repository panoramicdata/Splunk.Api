using Splunk.Api.Models.Search;

namespace Splunk.Api.IntegrationTest.Search;

[Collection(SplunkTestGroup.Name)]
public class SearchJobResultsIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	private async Task<string> CreateFinishedJobAsync()
	{
		var created = await Client.SearchJobs.CreateAsync(
			new SearchJobCreateRequest
			{
				Search = "search index=_internal | head 5",
				EarliestTime = "-24h",
				StatusBuckets = 300,
				ExecutionMode = SearchExecutionMode.Blocking,
				Id = SplunkFixture.UniqueName("res")
			},
			Ct);
		return created.Sid;
	}

	[Fact]
	public async Task ResultsEventsAndPreview_ReadTheFinishedJob()
	{
		var sid = await CreateFinishedJobAsync();
		try
		{
			var results = await Client.SearchJobResults.GetResultsAsync(sid, new SearchResultsOptions { Count = 3, Fields = ["host", "_raw"], AddSummaryToMetadata = true }, Ct);
			results.Results.Should().HaveCount(3);
			results.Fields.Select(f => f.Name).Should().Equal("host", "_raw");
			results.Fields[0].AdditionalProperties.Should().ContainKey("summary.count");
			results.Results[0].Raw.Should().NotBeNullOrEmpty();

			var page2 = await Client.SearchJobResults.GetResultsAsync(sid, new SearchResultsOptions { Count = 3, Offset = 3 }, Ct);
			page2.Results.Should().HaveCount(2);
			page2.InitOffset.Should().Be(3);

			var postProcessed = await Client.SearchJobResults.PostProcessResultsAsync(sid, new SearchResultsRequest { Search = "stats count", Count = 0 }, Ct);
			postProcessed.Results.Should().ContainSingle().Which["count"].Should().Be("5");

			var preview = await Client.SearchJobResults.GetPreviewAsync(sid, new SearchResultsOptions { Count = 1 }, Ct);
			preview.Preview.Should().BeFalse();
			preview.Results.Should().ContainSingle();

			var previewPost = await Client.SearchJobResults.PostProcessPreviewAsync(sid, new SearchResultsRequest { Search = "stats count" }, Ct);
			previewPost.Results.Should().ContainSingle();

			var events = await Client.SearchJobResults.GetEventsAsync(sid, new SearchEventsOptions { Count = 2, MaxLines = 1, TruncationMode = TruncationMode.Truncate }, Ct);
			events.Results.Should().HaveCount(2);
			events.Results[0].Time.Should().NotBeNull();

			var eventsPost = await Client.SearchJobResults.PostProcessEventsAsync(sid, new SearchEventsRequest { Count = 1, Fields = ["host"] }, Ct);
			eventsPost.Results.Should().ContainSingle().Which.FieldNames.Should().Contain("host");
		}
		finally
		{
			await Client.SearchJobs.DeleteAsync(sid, CancellationToken.None);
		}
	}

	[Fact]
	public async Task CsvAndRaw_StreamTheFinishedJob()
	{
		var sid = await CreateFinishedJobAsync();
		try
		{
			using (var csv = new StreamReader(await Client.SearchJobResults.GetResultsCsvAsync(sid, new SearchResultsOptions { Fields = ["host"] }, Ct)))
			{
				(await csv.ReadLineAsync(Ct)).Should().Be("host");
				(await csv.ReadToEndAsync(Ct)).Split('\n', StringSplitOptions.RemoveEmptyEntries).Should().HaveCount(5);
			}

			using var raw = new StreamReader(await Client.SearchJobResults.GetEventsRawAsync(sid, new SearchEventsOptions { Count = 2 }, Ct));
			(await raw.ReadToEndAsync(Ct)).Should().NotBeNullOrWhiteSpace();
		}
		finally
		{
			await Client.SearchJobs.DeleteAsync(sid, CancellationToken.None);
		}
	}
}
