using Splunk.Api.Models;
using Splunk.Api.Models.Search;
using System.Net;

namespace Splunk.Api.IntegrationTest.Search;

[Collection(SplunkTestGroup.Name)]
public class SearchJobsIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	[Fact]
	public async Task JobLifecycle_CreateInspectReadAndDelete()
	{
		var sid = SplunkFixture.UniqueName("job");
		var created = await Client.SearchJobs.CreateAsync(
			new SearchJobCreateRequest
			{
				Search = "search index=_internal | head 5",
				EarliestTime = "-24h",
				LatestTime = "now",
				StatusBuckets = 300,
				RequiredFields = ["host", "sourcetype"],
				Id = sid
			},
			Ct);
		try
		{
			created.Sid.Should().Be(sid);
			var job = await Client.Search.WaitForCompletionAsync(sid, new SearchWaitOptions { PollInterval = TimeSpan.FromMilliseconds(200), Timeout = TimeSpan.FromMinutes(2) }, Ct);
			job.Sid.Should().Be(sid);
			job.DispatchState.Should().Be(SearchDispatchState.Done);
			job.EventCount.Should().Be(5);
			job.Request.Should().ContainKey("search");

			var listed = await Client.SearchJobs.ListAsync(new ListOptions { Count = 0 }, Ct);
			listed.Entries.Should().Contain(e => e.Content!.Sid == sid);

			var updated = await Client.SearchJobs.UpdateAsync(sid, new SearchJobUpdateRequest([new("ticket", "INC-1")]), Ct);
			updated.Entries.Should().ContainSingle().Which.Content!.Sid.Should().Be(sid);

			var control = await Client.SearchJobs.ControlAsync(sid, new SearchJobControlRequest { Action = SearchJobAction.SetTtl, Ttl = 300 }, Ct);
			control.Messages.Should().ContainSingle().Which.Text.Should().Contain("300");

			var summary = await Client.SearchJobs.GetSummaryAsync(sid, new SearchJobSummaryOptions { Fields = ["sourcetype"] }, Ct);
			summary!.EventCount.Should().Be(5);
			summary.Fields.Should().ContainKey("sourcetype").WhoseValue.Count.Should().Be(5);

			var timeline = await Client.SearchJobs.GetTimelineAsync(sid, null, Ct);
			timeline!.Buckets.Sum(b => b.TotalCount).Should().Be(5);

			var log = await Client.SearchJobs.GetSearchLogAsync(sid, Ct);
			log.Should().Contain("INFO");
		}
		finally
		{
			await Client.SearchJobs.DeleteAsync(sid, CancellationToken.None);
		}

		var act = () => Client.SearchJobs.GetAsync(sid, Ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task ControlActions_AreAccepted()
	{
		var created = await Client.SearchJobs.CreateAsync(
			new SearchJobCreateRequest { Search = "search index=_internal | stats count by sourcetype", EarliestTime = "-30d", Id = SplunkFixture.UniqueName("ctl") },
			Ct);
		try
		{
			foreach (var action in new[] { SearchJobAction.Pause, SearchJobAction.Unpause, SearchJobAction.Touch, SearchJobAction.EnablePreview, SearchJobAction.DisablePreview })
			{
				var reply = await Client.SearchJobs.ControlAsync(created.Sid, new SearchJobControlRequest { Action = action }, Ct);
				reply.Messages.Should().NotBeEmpty(action.ToString());
			}

			(await Client.SearchJobs.ControlAsync(created.Sid, new SearchJobControlRequest { Action = SearchJobAction.SetPriority, Priority = 4 }, Ct)).Messages.Should().NotBeEmpty();
			(await Client.SearchJobs.ControlAsync(created.Sid, new SearchJobControlRequest { Action = SearchJobAction.Finalize }, Ct)).Messages.Should().NotBeEmpty();
		}
		finally
		{
			await Client.SearchJobs.ControlAsync(created.Sid, new SearchJobControlRequest { Action = SearchJobAction.Cancel }, CancellationToken.None);
		}
	}

	[Fact]
	public async Task RunOneshotAsync_ReturnsResultsInline()
	{
		var results = await Client.SearchJobs.RunOneshotAsync(
			new SearchOneshotRequest { Search = "| makeresults count=5 | eval x=random(), mv=split(\"a,b\", \",\")", Count = 0 },
			Ct);

		results.Results.Should().HaveCount(5);
		results.Fields.Select(f => f.Name).Should().Contain(["_time", "x", "mv"]);
		results.Results[0].GetValues("mv").Should().Equal("a", "b");
		results.Results[0].Time.Should().NotBeNull();
	}

	[Fact]
	public async Task CreateAsync_BadSearch_RaisesBadRequest()
	{
		var act = () => Client.SearchJobs.CreateAsync(new SearchJobCreateRequest { Search = "| nosuchcommand" }, Ct);

		var thrown = await act.Should().ThrowAsync<SplunkApiException>();
		thrown.Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		thrown.Which.Message.Should().Be("Unknown search command 'nosuchcommand'.");
	}
}
