using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public partial class SplunkSearchTests
{
	private const string Created = """{"sid":"s1"}""";
	private const string Deleted = """{"messages":[{"type":"INFO","text":"Search job cancelled."}]}""";
	private const string TwoRows = """{"preview":false,"init_offset":0,"messages":[{"type":"INFO","text":"page"}],"fields":[{"name":"n"}],"results":[{"n":"1"},{"n":"2"}]}""";
	private const string OneRow = """{"preview":false,"init_offset":2,"messages":[],"fields":[{"name":"n"}],"results":[{"n":"3"}]}""";

	private static readonly string Running = SearchRequestAssert.Feed("| makeresults", """{"sid":"s1","dispatchState":"RUNNING","isDone":false,"doneProgress":0.5}""");
	private static readonly string Done = SearchRequestAssert.Feed("| makeresults", """{"sid":"s1","dispatchState":"DONE","isDone":true,"resultCount":3}""");
	private static readonly string Failed = SearchRequestAssert.Feed("| makeresults", SearchJson.FailedJobContent);

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	/// <summary>A client whose search helper records its waits instead of sleeping, on a clock those waits advance.</summary>
	private static (SplunkClient Client, StubHandler Stub, List<TimeSpan> Waits) Create(params (HttpStatusCode Status, string Json)[] responses)
	{
		var stub = new StubHandler();
		foreach (var (status, json) in responses)
		{
			stub.Enqueue(status, json);
		}

		var client = TestClient.Create(stub);
		var clock = new ManualTime();
		var waits = new List<TimeSpan>();
		client.Search.TimeProvider = clock;
		client.Search.Delay = (wait, _) =>
		{
			waits.Add(wait);
			clock.Advance(wait);
			return Task.CompletedTask;
		};
		return (client, stub, waits);
	}

	private static (HttpStatusCode, string) Ok(string json) => (HttpStatusCode.OK, json);

	private static string[] Requests(StubHandler stub) => [.. stub.Calls.Select(c => $"{c.Method} {c.Uri.PathAndQuery}")];

	[Fact]
	public async Task RunAsync_CreatesPollsPagesAndDeletesTheJob()
	{
		var (client, stub, waits) = Create(Ok(Created), Ok(Running), Ok(Done), Ok(TwoRows), Ok(OneRow), Ok(Deleted));
		using var _ = client;

		var run = await client.Search.RunAsync(
			new SearchJobCreateRequest { Search = "| makeresults count=3" },
			new SearchRunOptions { PollInterval = TimeSpan.FromSeconds(2), PageSize = 2 },
			Ct);

		Requests(stub).Should().Equal(
			"POST /services/search/jobs?output_mode=json",
			"GET /services/search/jobs/s1?output_mode=json",
			"GET /services/search/jobs/s1?output_mode=json",
			"GET /services/search/v2/jobs/s1/results?count=2&offset=0&output_mode=json",
			"GET /services/search/v2/jobs/s1/results?count=2&offset=2&output_mode=json",
			"DELETE /services/search/jobs/s1?output_mode=json");
		stub.Calls[0].Body.Should().Be("search=%7C+makeresults+count%3D3");
		waits.Should().Equal(TimeSpan.FromSeconds(2));
		run.Job.ResultCount.Should().Be(3);
		run.Fields.Should().ContainSingle().Which.Name.Should().Be("n");
		run.Messages.Should().ContainSingle().Which.Text.Should().Be("page");
		run.Results.Select(r => r["n"]).Should().Equal("1", "2", "3");
	}

	[Fact]
	public async Task RunAsync_WithASearchString_UsesTheDefaults()
	{
		var (client, stub, waits) = Create(Ok(Created), Ok(Done), Ok(OneRow), Ok(Deleted));
		using var _ = client;

		var run = await client.Search.RunAsync("| makeresults", Ct);

		Requests(stub)[2].Should().Be("GET /services/search/v2/jobs/s1/results?count=10000&offset=0&output_mode=json");
		stub.Calls.Should().HaveCount(4);
		waits.Should().BeEmpty();
		run.Results.Should().ContainSingle();
	}

	[Fact]
	public async Task RunAsync_CanKeepTheJob()
	{
		var (client, stub, _) = Create(Ok(Created), Ok(Done), Ok(OneRow));
		using var _ = client;

		await client.Search.RunAsync(new SearchJobCreateRequest { Search = "| makeresults" }, new SearchRunOptions { DeleteJobWhenDone = false }, Ct);

		stub.Calls.Should().HaveCount(3);
		stub.Calls.Should().NotContain(c => c.Method == HttpMethod.Delete);
	}

	[Fact]
	public async Task RunAsync_FailedJob_ThrowsWithSplunksMessageAndDeletesTheJob()
	{
		var (client, stub, _) = Create(Ok(Created), Ok(Failed), Ok(Deleted));
		using var _ = client;

		var act = () => client.Search.RunAsync("| makeresults | eval x=nosuchfunc(1)", Ct);

		var thrown = await act.Should().ThrowAsync<SplunkSearchException>();
		thrown.Which.Message.Should().Be("Error in 'EvalCommand': The 'nosuchfunc' function is unsupported or undefined.");
		thrown.Which.Job!.Sid.Should().Be("failed_sid");
		thrown.Which.Messages.Should().HaveCount(2);
		Requests(stub)[^1].Should().Be("DELETE /services/search/jobs/s1?output_mode=json");
	}

	[Fact]
	public async Task RunAsync_DeleteFailure_DoesNotHideTheSearchFailure()
	{
		var (client, stub, _) = Create(Ok(Created), Ok(Failed), (HttpStatusCode.InternalServerError, """{"messages":[]}"""));
		using var _ = client;

		var act = () => client.Search.RunAsync("| makeresults", Ct);

		await act.Should().ThrowAsync<SplunkSearchException>();
		stub.Calls.Should().HaveCount(3);
	}

	[Fact]
	public async Task RunAsync_Timeout_ThrowsAndDeletesTheJob()
	{
		var (client, stub, waits) = Create(Ok(Created), Ok(Running), Ok(Running), Ok(Running), Ok(Deleted));
		using var _ = client;

		var act = () => client.Search.RunAsync(
			new SearchJobCreateRequest { Search = "| makeresults" },
			new SearchRunOptions { PollInterval = TimeSpan.FromSeconds(1), Timeout = TimeSpan.FromSeconds(2) },
			Ct);

		(await act.Should().ThrowAsync<TimeoutException>()).Which.Message.Should().Be("Search job 's1' did not finish within 00:00:02.");
		waits.Should().HaveCount(2);
		Requests(stub)[^1].Should().Be("DELETE /services/search/jobs/s1?output_mode=json");
	}

	[Fact]
	public async Task RunAsync_Cancelled_CancelsTheWaitAndDeletesTheJob()
	{
		var (client, stub, _) = Create(Ok(Created), Ok(Running), Ok(Deleted));
		using var _ = client;
		client.Search.Delay = (_, _) => throw new OperationCanceledException();

		var act = () => client.Search.RunAsync("| makeresults", Ct);

		await act.Should().ThrowAsync<OperationCanceledException>();
		Requests(stub)[^1].Should().Be("DELETE /services/search/jobs/s1?output_mode=json");
	}

	[Fact]
	public async Task RunAsync_RejectsANullRequest()
	{
		using var client = TestClient.Create(new StubHandler());

		var act = () => client.Search.RunAsync(null!, null, Ct);

		await act.Should().ThrowAsync<ArgumentNullException>();
	}
}
