using Splunk.Api.Models;
using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public partial class SplunkSearchTests
{
	[Fact]
	public async Task WaitForCompletionAsync_ZombieJob_Throws()
	{
		var zombie = SearchRequestAssert.Feed("x", """{"sid":"s1","dispatchState":"RUNNING","isZombie":true,"messages":[]}""");
		var (client, _, _) = Create(Ok(zombie));
		using var _ = client;

		var act = () => client.Search.WaitForCompletionAsync("s1", null, Ct);

		(await act.Should().ThrowAsync<SplunkSearchException>()).Which.Message.Should().Be("The search failed.");
	}

	[Fact]
	public async Task WaitForCompletionAsync_FailedStateWithoutTheFlag_Throws()
	{
		var failed = SearchRequestAssert.Feed("x", """{"sid":"s1","dispatchState":"FAILED","isFailed":false,"messages":[{"type":"WARN","text":"w"}]}""");
		var (client, _, _) = Create(Ok(failed));
		using var _ = client;

		var act = () => client.Search.WaitForCompletionAsync("s1", new SearchWaitOptions(), Ct);

		await act.Should().ThrowAsync<SplunkSearchException>();
	}

	[Fact]
	public async Task ReadResultsAsync_ReadsPageByPage()
	{
		var (client, stub, _) = Create(Ok(TwoRows), Ok(SearchJson.EmptyResults));
		using var _ = client;

		var rows = await client.Search.ReadResultsAsync("s1", 2, Ct).ToListAsync(Ct);

		rows.Select(r => r["n"]).Should().Equal("1", "2");
		Requests(stub)[1].Should().Be("GET /services/search/v2/jobs/s1/results?count=2&offset=2&output_mode=json");
	}

	[Fact]
	public async Task ReadResultsAsync_RejectsAPageSizeBelowOne()
	{
		using var client = TestClient.Create(new StubHandler());

		var act = async () => await client.Search.ReadResultsAsync("s1", 0, Ct).ToListAsync(Ct);

		await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
	}

	[Fact]
	public async Task OneshotAsync_ReturnsEveryResult()
	{
		var (client, stub, _) = Create(Ok(SearchJson.Results));
		using var _ = client;

		var results = await client.Search.OneshotAsync("| makeresults count=2", Ct);

		stub.Calls[0].Body.Should().Be("search=%7C+makeresults+count%3D2&exec_mode=oneshot&count=0");
		results.Results.Should().HaveCount(2);
	}

	[Fact]
	public async Task ExportAsync_YieldsOnlyFinalResults()
	{
		var stub = SearchRequestAssert.TextStub(SearchJson.Export, "application/json");
		using var client = TestClient.Create(stub);

		var rows = await client.Search.ExportAsync("search index=_internal | head 2", Ct).ToListAsync(Ct);

		stub.Calls[0].Uri.AbsolutePath.Should().Be("/services/search/v2/jobs/export");
		rows.Select(r => r.GetValues("x").Count).Should().Equal(1, 2);
	}

	[Fact]
	public async Task ExportAsync_ErrorInTheStream_Throws()
	{
		var stub = SearchRequestAssert.TextStub(
			"""
			{"preview":false,"offset":0,"result":{"x":"1"}}
			{"preview":false,"messages":[{"type":"FATAL","text":"Search auto-canceled"}]}
			""",
			"application/json");
		using var client = TestClient.Create(stub);
		var rows = new List<SearchResult>();

		var act = async () =>
		{
			await foreach (var row in client.Search.ExportAsync(new SearchExportRequest { Search = "search *" }, Ct))
			{
				rows.Add(row);
			}
		};

		(await act.Should().ThrowAsync<SplunkSearchException>()).Which.Job.Should().BeNull();
		rows.Should().ContainSingle();
	}

	[Fact]
	public void SplunkSearchException_WithoutAnErrorMessage_SaysTheSearchFailed()
	{
		var exception = new SplunkSearchException([new SplunkMessage { Type = "ERROR", Text = string.Empty }], null);

		exception.Message.Should().Be("The search failed.");
	}

	/// <summary>A clock that only moves when told to.</summary>
	private sealed class ManualTime : TimeProvider
	{
		private long _ticks;

		public override long TimestampFrequency => TimeSpan.TicksPerSecond;

		public override long GetTimestamp() => _ticks;

		public void Advance(TimeSpan by) => _ticks += by.Ticks;
	}
}
