using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ShClusterCaptainArtifactsTests
{
	private const string Path = "/services/shcluster/captain/artifacts";
	private const string Sid = "scheduler__nobody__simplexml__RMD5dc07327042a35a17_at_1469214000_37_11111111-1111-1111-1111-111111111111";

	// From the reference's example.
	private const string ArtifactContent = """
		{
			"artifact_size": "77824",
			"eai:acl": null,
			"label": "timechart_scheduled",
			"origin_guid": "11111111-1111-1111-1111-111111111111",
			"peers": {
				"11111111-1111-1111-1111-111111111111": { "directory_path": "/opt/splunk/var/run/splunk/dispatch/sid", "status": "Complete" }
			},
			"perms": "read : [ *, splunk-system-user ], write : [ admin, power, splunk-system-user ]",
			"service_after_time": "0",
			"user": "splunk-system-user"
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGetWithRemoteSids()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptainArtifacts.ListAsync(new ShClusterArtifactListOptions { RemoteSids = true }, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path, "?remote_sids=true&output_mode=json");

	[Fact]
	public async Task GetAsync_SendsGetWithTheSid()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptainArtifacts.GetAsync(Sid, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/{Sid}");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var artifact = await RequestProbe.ReadContentAsync((c, ct) => c.ShClusterCaptainArtifacts.GetAsync(Sid, ct), Sid, ArtifactContent);

		artifact.ArtifactSize.Should().Be(77824);
		artifact.Label.Should().Be("timechart_scheduled");
		artifact.OriginGuid.Should().Be("11111111-1111-1111-1111-111111111111");
		var replica = artifact.Peers["11111111-1111-1111-1111-111111111111"];
		replica.DirectoryPath.Should().Be("/opt/splunk/var/run/splunk/dispatch/sid");
		replica.Status.Should().Be("Complete");
		artifact.Permissions.Should().StartWith("read : [");
		artifact.ServiceAfterTime.Should().BeNull();
		artifact.User.Should().Be("splunk-system-user");
	}

	[Fact]
	public Task ListAsync_NoShc_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ShClusterCaptainArtifacts.ListAsync(null, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ShcNotEnabled,
			"Search Head Clustering is not enabled on this node. REST endpoint is not available");
}
