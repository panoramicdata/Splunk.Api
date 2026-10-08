using Splunk.Api.Models.Deployment;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class DistributedSearchTests
{
	private const string Path = "/services/search/distributed";

	// Captured from Splunk Enterprise 10.6.0.5, with the reference's list values added.
	private const string ConfigContent = """
		{
			"blacklistNames": "*.tmp",
			"blacklistURLs": "http://example.com/*",
			"checkTimedOutServersFrequency": "60",
			"connectionTimeout": 10,
			"disabled": false,
			"dist_search_enabled": true,
			"eai:acl": null,
			"receiveTimeout": 600,
			"removedTimedOutServers": "0",
			"sendTimeout": 30,
			"serverTimeout": 10,
			"servers": "idx01.example.com:8089",
			"shareBundles": "1",
			"statusTimeout": 10
		}
		""";

	// From the reference's example (a 10.6 standalone has no search peers); host names replaced.
	private const string PeerContent = """
		{
			"build": "86587d4e3b27",
			"bundle_versions": ["13134207368020721783"],
			"disabled": "0",
			"eai:acl": null,
			"guid": "00000000-0000-0000-0000-000000000003",
			"is_https": "1",
			"licenseSignature": "abc123",
			"peerName": "idx01",
			"peerType": "configured",
			"replicationStatus": "Successful",
			"status": "Up",
			"status_details": "OK",
			"version": "10.6.0.5"
		}
		""";

	[Fact]
	public async Task GetConfigAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.DistributedSearch.GetConfigAsync(ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/config");

	[Fact]
	public async Task ListPeersAsync_SendsGet()
		=> (await RequestProbe.SendAsync((c, ct) => c.DistributedSearch.ListPeersAsync(null, ct)))
			.ShouldBeProbed(HttpMethod.Get, $"{Path}/peers");

	[Fact]
	public async Task AddPeerAsync_SendsPostWithThePeer()
		=> (await RequestProbe.SendAsync((c, ct) => c.DistributedSearch.AddPeerAsync(
			new DistributedPeerCreateRequest { Name = "idx01:8089", RemoteUsername = "admin", RemotePassword = "p@ss" },
			ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/peers", body: "name=idx01%3A8089&remoteUsername=admin&remotePassword=p%40ss");

	[Fact]
	public async Task UpdatePeerAsync_SendsPostWithTheCredentials()
		=> (await RequestProbe.SendAsync((c, ct) => c.DistributedSearch.UpdatePeerAsync(
			"idx01:8089",
			new DistributedPeerUpdateRequest { RemoteUsername = "admin", RemotePassword = "pw" },
			ct)))
			.ShouldBeProbed(HttpMethod.Post, $"{Path}/peers/idx01%3A8089", body: "remoteUsername=admin&remotePassword=pw");

	[Fact]
	public async Task GetConfigAsync_MapsEveryModelledField()
	{
		var config = await RequestProbe.ReadContentAsync((c, ct) => c.DistributedSearch.GetConfigAsync(ct), "distributedSearch", ConfigContent);

		config.BlacklistNames.Should().Be("*.tmp");
		config.BlacklistUrls.Should().Be("http://example.com/*");
		config.CheckTimedOutServersFrequency.Should().Be(60);
		config.ConnectionTimeout.Should().Be(10);
		config.Disabled.Should().BeFalse();
		config.DistributedSearchEnabled.Should().BeTrue();
		config.ReceiveTimeout.Should().Be(600);
		config.RemovedTimedOutServers.Should().BeFalse();
		config.SendTimeout.Should().Be(30);
		config.ServerTimeout.Should().Be(10);
		config.Servers.Should().Be("idx01.example.com:8089");
		config.ShareBundles.Should().BeTrue();
		config.StatusTimeout.Should().Be(10);
	}

	[Fact]
	public async Task ListPeersAsync_MapsEveryModelledField()
	{
		var peer = await RequestProbe.ReadContentAsync((c, ct) => c.DistributedSearch.ListPeersAsync(null, ct), "idx01:8089", PeerContent);

		peer.Build.Should().Be("86587d4e3b27");
		peer.BundleVersions.Should().Equal("13134207368020721783");
		peer.Disabled.Should().BeFalse();
		peer.PeerGuid.Should().Be("00000000-0000-0000-0000-000000000003");
		peer.IsHttps.Should().BeTrue();
		peer.LicenseSignature.Should().Be("abc123");
		peer.PeerName.Should().Be("idx01");
		peer.PeerType.Should().Be("configured");
		peer.ReplicationStatus.Should().Be("Successful");
		peer.Status.Should().Be("Up");
		peer.Version.Should().Be("10.6.0.5");
		peer.AdditionalProperties["status_details"].GetString().Should().Be("OK");
	}

	[Fact]
	public Task AddPeerAsync_Error_RaisesSplunkApiException()
		=> RequestProbe.FailsAsync(
			(c, ct) => c.DistributedSearch.AddPeerAsync(new DistributedPeerCreateRequest { Name = "x:1", RemoteUsername = "u", RemotePassword = "p" }, ct),
			HttpStatusCode.BadRequest,
			"""{"messages":[{"type":"ERROR","text":"Error while sending public key to search peer"}]}""",
			"Error while sending public key to search peer");
}
