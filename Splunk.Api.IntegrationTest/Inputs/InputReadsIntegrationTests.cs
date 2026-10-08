using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;

namespace Splunk.Api.IntegrationTest.Inputs;

/// <summary>Reads every input family the shared standalone instance has.</summary>
[Collection(SplunkTestGroup.Name)]
public class InputReadsIntegrationTests(SplunkFixture fixture)
{
	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private static readonly ListOptions All = new() { Count = 0 };

	[Fact]
	public async Task MonitorInputs_ListGetAndMembers()
	{
		var feed = await fixture.Client.MonitorInputs.ListAsync(All, Ct);

		var splunkLogs = feed.Entries.Should().Contain(e => e.Name == "$SPLUNK_HOME/var/log/splunk").Subject;
		splunkLogs.Content!.Index.Should().Be("_internal");
		var single = await fixture.Client.MonitorInputs.GetAsync(splunkLogs.Name, Ct);
		single.Entries.Should().ContainSingle().Which.Content!.Index.Should().Be("_internal");
		var members = await ListMembersAsync(splunkLogs.Name);
		members.Should().NotBeEmpty().And.OnlyContain(e => e.Name.StartsWith('/'));
	}

	/// <summary>
	/// Lists a monitor input's files. The list is briefly empty while Splunk reloads its file monitor after any monitor
	/// input changes (as other tests do), so an empty answer is retried for a few seconds.
	/// </summary>
	private async Task<IReadOnlyList<SplunkEntry<SplunkDynamicContent>>> ListMembersAsync(string name)
	{
		for (var attempt = 1; ; attempt++)
		{
			var members = (await fixture.Client.MonitorInputs.ListMembersAsync(name, new ListOptions { Count = 5 }, Ct)).Entries;
			if (members.Count > 0 || attempt == 20)
			{
				return members;
			}

			await Task.Delay(TimeSpan.FromMilliseconds(500), Ct);
		}
	}

	[Fact]
	public async Task ScriptedInputs_ListAndGet()
	{
		var feed = await fixture.Client.ScriptedInputs.ListAsync(All, Ct);

		var first = feed.Entries.Should().NotBeEmpty().And.Subject.First();
		first.Content!.Interval.Should().NotBeNullOrEmpty();
		(await fixture.Client.ScriptedInputs.GetAsync(first.Name, Ct)).Entries.Should().ContainSingle().Which.Name.Should().Be(first.Name);
	}

	[Fact]
	public async Task CookedTcpInputs_ListGetAndConnections()
	{
		var feed = await fixture.Client.CookedTcpInputs.ListAsync(null, Ct);

		feed.Entries.Should().Contain(e => e.Name == "9997").Which.Content!.Group.Should().Be("listenerports");
		(await fixture.Client.CookedTcpInputs.GetAsync("9997", Ct)).Entries.Should().ContainSingle();
		(await fixture.Client.CookedTcpInputs.ListConnectionsAsync("9997", Ct)).Entries.Should().BeEmpty("nothing forwards to the test instance");
	}

	[Fact]
	public async Task NetworkInputLists_Succeed()
	{
		(await fixture.Client.RawTcpInputs.ListAsync(null, Ct)).Paging.Should().NotBeNull();
		(await fixture.Client.UdpInputs.ListAsync(null, Ct)).Paging.Should().NotBeNull();
		(await fixture.Client.SplunkTcpTokens.ListAsync(null, Ct)).Paging.Should().NotBeNull();
		(await fixture.Client.OneshotInputs.ListAsync(null, Ct)).Paging.Should().NotBeNull();
	}

	[Fact]
	public async Task TcpSslSettings_ListAndGet()
	{
		var feed = await fixture.Client.TcpSslSettings.ListAsync(null, Ct);

		var ssl = feed.Entries.Should().ContainSingle().Subject;
		ssl.Name.Should().BeEmpty();
		ssl.Content!.SslVersions.Should().NotBeNullOrEmpty();
		(await fixture.Client.TcpSslSettings.GetAsync("ssl", Ct)).Entries.Should().ContainSingle().Which.Content!.CipherSuite.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task AllInputs_ListAndGet()
	{
		var feed = await fixture.Client.AllInputs.ListAsync(new DataInputListOptions { Count = 0, Common = true }, Ct);

		feed.Entries.Should().Contain(e => e.Content!.Kind == "monitor");
		var cooked = await fixture.Client.AllInputs.GetAsync("9997", new DataInputOptions { Common = false }, Ct);
		cooked.Entries.Should().ContainSingle().Which.Content!.Kind.Should().Be("cooked");
	}

	[Fact]
	public async Task ModularInputs_ListAndGet()
	{
		var feed = await fixture.Client.ModularInputs.ListAsync(All, Ct);

		feed.Entries.Should().Contain(e => e.Name == "journald");
		var journald = (await fixture.Client.ModularInputs.GetAsync("journald", Ct)).Entries.Should().ContainSingle().Subject.Content!;
		journald.StreamingMode.Should().NotBeNullOrEmpty();
		journald.Endpoint!.Arguments.Should().ContainKey("name");
	}

	[Fact]
	public async Task HecReads_Succeed()
	{
		(await fixture.Client.HecTokens.ListAsync(null, Ct)).Paging.Should().NotBeNull();
		var settings = (await fixture.Client.HecTokens.GetSettingsAsync(HecSettingsUpdateRequest.SettingsName, Ct)).Entries.Should().ContainSingle().Subject;
		settings.Name.Should().Be("http");
		settings.Content!.Port.Should().Be(8088);
		(await fixture.Client.HecConnections.ListAsync(Ct)).Paging.Should().NotBeNull();
	}

	[Fact]
	public async Task HecConnections_UnknownSender_IsNotFound()
	{
		var act = () => fixture.Client.HecConnections.GetAsync("192.0.2.77", Ct);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
	}

	[Fact]
	public async Task PipelineSets_List()
	{
		var feed = await fixture.Client.PipelineSets.ListAsync(Ct);

		feed.Entries.Should().Contain(e => e.Name == "ingest_pipe_0").Which.Content!.BusiestThreadName.Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task IngestReads_Succeed()
	{
		(await fixture.Client.IngestDestinations.ListAsync(null, Ct)).Generator.Should().NotBeNull();
		(await fixture.Client.IngestRulesets.ListAsync(Ct)).Generator.Should().NotBeNull();
		(await fixture.Client.IndexingPreviews.ListAsync(Ct)).Generator.Should().NotBeNull();
	}

	[Fact]
	public async Task IngestRulesets_Publish_OnlyAClusterManagerAccepts()
	{
		var act = () => fixture.Client.IngestRulesets.PublishAsync(Ct);

		(await act.Should().ThrowAsync<SplunkApiException>()).Which.Message.Should().Contain("non cluster manager");
	}
}
