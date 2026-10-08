using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SearchSchedulerTests
{
	private static readonly string StatusFeed = SearchRequestAssert.Feed("status", """{"eai:acl":null,"saved_searches_disabled":"1"}""", "system");

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task GetStatusAsync_SendsGet()
	{
		var stub = TestClient.Stub(StatusFeed);
		using var client = TestClient.Create(stub);

		await client.SearchScheduler.GetStatusAsync(Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Get, "/services/search/scheduler", "?output_mode=json");
	}

	[Fact]
	public async Task GetStatusAsync_MapsTheState()
	{
		using var client = TestClient.Create(TestClient.Stub(StatusFeed));

		var feed = await client.SearchScheduler.GetStatusAsync(Ct);

		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Name.Should().Be("status");
		entry.Content!.SavedSearchesDisabled.Should().BeTrue();
	}

	[Fact]
	public async Task SetStatusAsync_SendsDisabled()
	{
		var stub = TestClient.Stub("""{"entry":[]}""");
		using var client = TestClient.Create(stub);

		var feed = await client.SearchScheduler.SetStatusAsync(new SearchSchedulerStatusRequest { Disabled = false }, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, "/services/search/scheduler/status", "?output_mode=json", "disabled=false");
		feed.Entries.Should().BeEmpty();
	}

	[Fact]
	public async Task SetStatusAsync_Error_RaisesSplunkApiException()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"You do not have permission."}]}""", HttpStatusCode.Forbidden));

		await SearchRequestAssert.FailsWith(() => client.SearchScheduler.SetStatusAsync(new SearchSchedulerStatusRequest { Disabled = true }, Ct), HttpStatusCode.Forbidden, "You do not have permission.");
	}
}
