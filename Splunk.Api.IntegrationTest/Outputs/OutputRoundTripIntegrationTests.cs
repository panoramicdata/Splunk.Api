using Splunk.Api.Models.Outputs;
using System.Net;

namespace Splunk.Api.IntegrationTest.Outputs;

/// <summary>
/// Forwarding configuration round trips at documentation addresses (192.0.2.0/24, never routed), run against the
/// collector test container: Splunk starts forwarding to a new group or receiver, which must not touch the shared
/// instance.
/// </summary>
[Collection(SplunkHecTestGroup.Name)]
public class OutputRoundTripIntegrationTests(SplunkHecFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Management;

	[Fact]
	public async Task Group_RoundTrip()
	{
		var name = SplunkFixture.UniqueName("group");
		var groups = Client.TcpOutputGroups;
		try
		{
			var created = await groups.CreateAsync(new TcpOutputGroupCreateRequest { Name = name, Servers = "192.0.2.1:9997", Method = "autobalance", HeartbeatFrequency = 45 }, Ct);
			created.Entries.Should().ContainSingle().Which.Content!.Servers.Should().Equal("192.0.2.1:9997");

			await groups.UpdateAsync(name, new TcpOutputGroupUpdateRequest { Servers = "192.0.2.1:9997,192.0.2.2:9997", HeartbeatFrequency = 60 }, Ct);

			var read = (await groups.GetAsync(name, Ct)).Entries.Single().Content!;
			read.Servers.Should().Equal("192.0.2.1:9997", "192.0.2.2:9997");
			read.HeartbeatFrequency.Should().Be(60);
			(await groups.ListAsync(null, Ct)).Entries.Should().Contain(e => e.Name == name);
		}
		finally
		{
			await groups.DeleteAsync(name, Ct);
		}

		(await Client.TcpOutputDefaults.GetAsync("tcpout", Ct)).Entries.Single().Content!.DefaultGroup.Should().BeNullOrEmpty();
	}

	[Fact]
	public async Task Server_RoundTrip()
	{
		const string name = "192.0.2.3:9997";
		var servers = Client.TcpOutputServers;
		try
		{
			var created = await servers.CreateAsync(new TcpOutputServerCreateRequest { Name = name, Disabled = true, SslVerifyServerCert = false }, Ct);
			created.Entries.Should().ContainSingle().Which.Content!.DestinationPort.Should().Be(9997);

			await servers.UpdateAsync(name, new TcpOutputServerUpdateRequest { SslVerifyServerCert = false, Disabled = true }, Ct);

			var read = (await servers.GetAsync(name, Ct)).Entries.Single().Content!;
			read.DestinationIp.Should().Be("192.0.2.3");
			read.Status.Should().NotBeNullOrEmpty();
			(await servers.ListConnectionsAsync(name, Ct)).Generator.Should().NotBeNull();
			(await servers.ListAsync(null, Ct)).Entries.Should().Contain(e => e.Name == name);
		}
		finally
		{
			await servers.DeleteAsync(name, Ct);
		}

		var act = () => servers.GetAsync(name, Ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Syslog_RoundTrip()
	{
		var name = SplunkFixture.UniqueName("syslog");
		var syslog = Client.SyslogOutputs;
		try
		{
			var created = await syslog.CreateAsync(new SyslogOutputCreateRequest { Name = name, Server = "192.0.2.1:514", Type = "udp", Priority = 13, Disabled = true }, Ct);
			created.Entries.Should().ContainSingle().Which.Content!.Priority.Should().Be(13);

			await syslog.UpdateAsync(name, new SyslogOutputUpdateRequest { Server = "192.0.2.1:514", Type = "tcp", TimestampFormat = "%b %e %H:%M:%S" }, Ct);

			var read = (await syslog.GetAsync(name, Ct)).Entries.Single().Content!;
			read.Type.Should().Be("tcp");
			read.Server.Should().Be("192.0.2.1:514");
			read.TimestampFormat.Should().Be("%b %e %H:%M:%S");
			(await syslog.ListAsync(null, Ct)).Entries.Should().Contain(e => e.Name == name);
		}
		finally
		{
			await syslog.DeleteAsync(name, Ct);
		}

		var act = () => syslog.GetAsync(name, Ct);
		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task Defaults_RoundTrip()
	{
		var defaults = Client.TcpOutputDefaults;
		try
		{
			(await defaults.CreateAsync(new TcpOutputDefaultsCreateRequest { Name = "tcpout", HeartbeatFrequency = 31 }, Ct))
				.Entries.Single().Content!.HeartbeatFrequency.Should().Be(31);
			(await defaults.UpdateAsync("tcpout", new TcpOutputDefaultsUpdateRequest { HeartbeatFrequency = 30 }, Ct))
				.Entries.Single().Content!.HeartbeatFrequency.Should().Be(30);

			await defaults.DeleteAsync("tcpout", Ct);

			(await defaults.GetAsync("tcpout", Ct)).Entries.Single().Content!.Disabled.Should().BeTrue("deleting the default settings disables them");
		}
		finally
		{
			await defaults.UpdateAsync("tcpout", new TcpOutputDefaultsUpdateRequest { Disabled = false, HeartbeatFrequency = 30 }, Ct);
		}

		(await defaults.ListAsync(null, Ct)).Entries.Single().Content!.Disabled.Should().BeFalse();
	}
}
