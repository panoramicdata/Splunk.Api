using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ShClusterCaptainJobsTests
{
	private const string Path = "/services/shcluster/captain/jobs";

	// From the reference's examples: a listed job with its attempts, and a single job.
	private const string ListedJobContent = """
		{
			"ATTEMPT_1": { "dispatch_time": "1413363600", "errormsg": "Application does not exist: SA-nix", "peer": "9999", "sid": "NO_SID_RECEIVED_YET", "success": "0" },
			"eai:acl": null,
			"job_state": "COMPLETED",
			"saved_search": "Alert - syslog errors last hour",
			"savedsearchtype": "savedsearch",
			"search_app": "SA-nix",
			"search_owner": "admin"
		}
		""";

	private const string SingleJobContent = """
		{
			"dispatch_time": "1469214120",
			"job_state": "COMPLETED",
			"peer": "11111111-1111-1111-1111-111111111111",
			"peer_scheme_host_port": "https://sh1:8089",
			"peer_servername": "sh-1",
			"saved_search": "timechart",
			"savedsearchtype": "scheduled",
			"search_app": "testing",
			"search_owner": "nobody",
			"sid": "scheduler__nobody__testing__RMD5_at_1469214120_39",
			"success": "1"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptainJobs.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task GetAsync_SendsGetWithTheEscapedName()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptainJobs.GetAsync("scheduled_my job", ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/scheduled_my%20job");

	[Fact]
	public async Task ListAsync_MapsAJobWithItsAttempts()
	{
		var job = await RequestProbe.ReadContentAsync((c, ct) => c.ShClusterCaptainJobs.ListAsync(null, ct), "savedsearch_x_1", ListedJobContent);

		job.JobState.Should().Be("COMPLETED");
		job.SavedSearch.Should().Be("Alert - syslog errors last hour");
		job.SavedSearchType.Should().Be("savedsearch");
		job.SearchApp.Should().Be("SA-nix");
		job.SearchOwner.Should().Be("admin");
		job.AdditionalProperties["ATTEMPT_1"].GetProperty("sid").GetString().Should().Be("NO_SID_RECEIVED_YET");
	}

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var job = await RequestProbe.ReadContentAsync((c, ct) => c.ShClusterCaptainJobs.GetAsync("scheduled_x", ct), "scheduled_x", SingleJobContent);

		job.DispatchTime.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1469214120));
		job.Peer.Should().Be("11111111-1111-1111-1111-111111111111");
		job.PeerSchemeHostPort.Should().Be("https://sh1:8089");
		job.PeerServerName.Should().Be("sh-1");
		job.SearchId.Should().Be("scheduler__nobody__testing__RMD5_at_1469214120_39");
		job.Success.Should().BeTrue();
	}

	[Fact]
	public Task ListAsync_NoShc_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ShClusterCaptainJobs.ListAsync(null, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ShcNotEnabled,
			"Search Head Clustering is not enabled on this node. REST endpoint is not available");
}
