using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public partial class SearchJobsTests
{
	[Fact]
	public async Task UpdateAsync_SendsCustomProperties()
	{
		var stub = TestClient.Stub(JobFeed);
		using var client = TestClient.Create(stub);

		var feed = await client.SearchJobs.UpdateAsync("my_sid", new SearchJobUpdateRequest([new("ticket", "INC-1"), new("owner team", "ops")]), Ct);

		stub.ShouldHaveSent(HttpMethod.Post, "/services/search/jobs/my_sid", "?output_mode=json", "custom.ticket=INC-1&custom.owner+team=ops");
		feed.Entries.Should().ContainSingle().Which.Content!.Sid.Should().Be("splunk_api_probe_2");
	}

	[Fact]
	public void SearchJobUpdateRequest_RejectsNull()
	{
		var act = () => new SearchJobUpdateRequest(null!);

		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public async Task DeleteAsync_SendsDelete()
	{
		var stub = TestClient.Stub("""{"messages":[{"type":"INFO","text":"Search job cancelled."}]}""");
		using var client = TestClient.Create(stub);

		await client.SearchJobs.DeleteAsync("my_sid", Ct);

		stub.ShouldHaveSent(HttpMethod.Delete, "/services/search/jobs/my_sid", "?output_mode=json");
	}

	[Theory]
	[InlineData(SearchJobAction.Pause, "action=pause")]
	[InlineData(SearchJobAction.Unpause, "action=unpause")]
	[InlineData(SearchJobAction.Finalize, "action=finalize")]
	[InlineData(SearchJobAction.Cancel, "action=cancel")]
	[InlineData(SearchJobAction.Touch, "action=touch")]
	[InlineData(SearchJobAction.EnablePreview, "action=enablepreview")]
	[InlineData(SearchJobAction.DisablePreview, "action=disablepreview")]
	public async Task ControlAsync_SendsTheAction(SearchJobAction action, string body)
	{
		var stub = TestClient.Stub("""{"messages":[]}""");
		using var client = TestClient.Create(stub);

		await client.SearchJobs.ControlAsync("my_sid", new SearchJobControlRequest { Action = action }, Ct);

		stub.ShouldHaveSent(HttpMethod.Post, "/services/search/jobs/my_sid/control", "?output_mode=json", body);
	}

	[Fact]
	public async Task ControlAsync_SendsArgumentsAndMapsTheMessages()
	{
		var stub = TestClient.Stub("""{"messages":[{"type":"INFO","text":"The ttl of the search job was changed to 120."}]}""");
		using var client = TestClient.Create(stub);

		var reply = await client.SearchJobs.ControlAsync(
			"my_sid",
			new SearchJobControlRequest { Action = SearchJobAction.SetTtl, Ttl = 120, Priority = 3, WorkloadPool = "pool" },
			Ct);

		stub.ShouldHaveSent(HttpMethod.Post, "/services/search/jobs/my_sid/control", "?output_mode=json", "action=setttl&ttl=120&priority=3&workload_pool=pool");
		reply.Messages.Should().ContainSingle().Which.Text.Should().Be("The ttl of the search job was changed to 120.");
	}

	[Theory]
	[InlineData(SearchJobAction.SetPriority, "action=setpriority")]
	[InlineData(SearchJobAction.SetWorkloadPool, "action=setworkloadpool")]
	public async Task ControlAsync_SendsTheArgumentActions(SearchJobAction action, string body)
	{
		var stub = TestClient.Stub("""{"messages":[]}""");
		using var client = TestClient.Create(stub);

		await client.SearchJobs.ControlAsync("my_sid", new SearchJobControlRequest { Action = action }, Ct);

		stub.Calls[0].Body.Should().Be(body);
	}

	[Fact]
	public async Task GetSearchLogAsync_SendsGetAndReturnsTheText()
	{
		var stub = SearchRequestAssert.TextStub("10-08-2026 13:47:15.569 INFO  dispatchRunner - Search process mode: preforked\n", "text/plain");
		using var client = TestClient.Create(stub);

		var log = await client.SearchJobs.GetSearchLogAsync("my_sid", Ct);

		stub.ShouldHaveSent(HttpMethod.Get, "/services/search/jobs/my_sid/search.log", "?output_mode=json");
		log.Should().StartWith("10-08-2026 13:47:15.569 INFO  dispatchRunner");
	}
}
