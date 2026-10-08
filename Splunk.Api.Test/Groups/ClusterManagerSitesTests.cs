using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class ClusterManagerSitesTests
{
	private const string Path = "/services/cluster/manager/sites";

	// From the reference's example.
	private const string SiteContent = """
		{
			"eai:acl": null,
			"peers": {
				"29F9560E-A44A-425C-8753-1C6158B46C84": { "host_port_pair": "10.0.1.1:8092", "server_name": "s1p3" },
				"61666763-43E9-411B-9464-D80A5119EF0E": { "host_port_pair": "10.0.1.1:8091", "server_name": "s1p2" }
			}
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerSites.ListAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, Path);

	[Fact]
	public async Task GetAsync_SendsGetWithTheSite()
		=> (await RequestProbe.SendAsync((c, ct) => c.ClusterManagerSites.GetAsync("site1", ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/site1");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var site = await RequestProbe.ReadContentAsync((c, ct) => c.ClusterManagerSites.GetAsync("site1", ct), "site1", SiteContent);

		site.Peers.Should().HaveCount(2);
		var peer = site.Peers["29F9560E-A44A-425C-8753-1C6158B46C84"];
		peer.HostPortPair.Should().Be("10.0.1.1:8092");
		peer.ServerName.Should().Be("s1p3");
	}

	[Fact]
	public Task GetAsync_NotAManager_RaisesServiceUnavailable()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.ClusterManagerSites.GetAsync("site1", ct),
			HttpStatusCode.ServiceUnavailable,
			ClusterErrors.ManagerNotEnabled,
			"Cluster manager is not enabled on this node");
}
