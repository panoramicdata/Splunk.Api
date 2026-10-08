namespace Splunk.Api.IntegrationTest.Outputs;

/// <summary>
/// Reads the shared instance's forwarding configuration. Writes run against the collector test container only:
/// creating a group or receiver makes Splunk forward to it, which would disturb the shared instance.
/// </summary>
[Collection(SplunkTestGroup.Name)]
public class OutputReadsIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task TcpOutputDefaults_ListAndGet()
	{
		(await fixture.Client.TcpOutputDefaults.ListAsync(null, Ct)).Entries.Should().ContainSingle().Which.Name.Should().Be("tcpout");

		var defaults = (await fixture.Client.TcpOutputDefaults.GetAsync("tcpout", Ct)).Entries.Should().ContainSingle().Subject.Content!;
		defaults.HeartbeatFrequency.Should().BePositive();
		defaults.MaxQueueSize.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task ForwardingLists_Succeed()
	{
		(await fixture.Client.TcpOutputGroups.ListAsync(null, Ct)).Paging.Should().NotBeNull();
		(await fixture.Client.TcpOutputServers.ListAsync(null, Ct)).Paging.Should().NotBeNull();
		(await fixture.Client.SyslogOutputs.ListAsync(null, Ct)).Paging.Should().NotBeNull();
	}
}
