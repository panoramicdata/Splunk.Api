using Splunk.Api.Models.Cluster;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ShClusterCaptainMembersTests
{
	private const string Path = "/services/shcluster/captain/members";
	private const string MemberGuid = "33333333-3333-3333-3333-333333333333";

	// From the reference's example.
	private const string MemberContent = """
		{
			"adhoc_searchhead": "0",
			"advertise_restart_required": "0",
			"artifact_count": "6",
			"delayed_artifacts_to_discard": [],
			"eai:acl": null,
			"fixup_set": [],
			"host_port_pair": "sh3:8089",
			"is_captain": "1",
			"kv_store_host_port": "sh3:8191",
			"label": "sh-3",
			"last_heartbeat": "1469221966",
			"mgmt_uri": "https://sh3:8089",
			"no_artifact_replications": "0",
			"peer_scheme_host_port": "https://sh3:8089",
			"pending_job_count": "1",
			"preferred_captain": "1",
			"replication_count": "2",
			"replication_port": "12243",
			"replication_use_ssl": "1",
			"site": "default",
			"status": "Up",
			"status_counter": { "Complete": "6", "PendingDiscard": "0" }
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptainMembers.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task GetAsync_SendsGetWithTheGuid()
		=> (await RequestProbe.SendAsync((c, ct) => c.ShClusterCaptainMembers.GetAsync(MemberGuid, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/{MemberGuid}");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var member = await RequestProbe.ReadContentAsync((c, ct) => c.ShClusterCaptainMembers.GetAsync(MemberGuid, ct), MemberGuid, MemberContent);

		member.AdhocSearchHead.Should().BeFalse();
		member.AdvertiseRestartRequired.Should().BeFalse();
		member.ArtifactCount.Should().Be(6);
		member.DelayedArtifactsToDiscard.Should().BeEmpty();
		member.FixupSet.Should().BeEmpty();
		member.HostPortPair.Should().Be("sh3:8089");
		member.IsCaptain.Should().BeTrue();
		member.KvStoreHostPort.Should().Be("sh3:8191");
		member.Label.Should().Be("sh-3");
		member.LastHeartbeat.Should().Be(DateTimeOffset.FromUnixTimeSeconds(1469221966));
		member.ManagementUri.Should().Be("https://sh3:8089");
		member.NoArtifactReplications.Should().BeFalse();
		member.PeerSchemeHostPort.Should().Be("https://sh3:8089");
		member.PendingJobCount.Should().Be(1);
		member.PreferredCaptain.Should().BeTrue();
		member.ReplicationCount.Should().Be(2);
		member.ReplicationPort.Should().Be(12243);
		member.ReplicationUseSsl.Should().BeTrue();
		member.Site.Should().Be("default");
		member.Status.Should().Be(ClusterPeerStatus.Up);
		member.StatusCounter["Complete"].Should().Be(6);
	}

	[Fact]
	public Task GetAsync_NoShc_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ShClusterCaptainMembers.GetAsync(MemberGuid, ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ShcNotEnabled,
			"Search Head Clustering is not enabled on this node. REST endpoint is not available");
}
