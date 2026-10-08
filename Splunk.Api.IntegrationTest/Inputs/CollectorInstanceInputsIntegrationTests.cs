using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;
using System.Net;
using System.Text;

namespace Splunk.Api.IntegrationTest.Inputs;

/// <summary>
/// Input writes that index data or change instance-wide settings, run against the collector test container rather
/// than the shared instance.
/// </summary>
[Collection(SplunkHecTestGroup.Name)]
public class CollectorInstanceInputsIntegrationTests(SplunkHecFixture fixture)
{
	private const string VersionFile = "/opt/splunk/etc/splunk.version";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Management;

	[Fact]
	public async Task Receivers_IndexTheBody()
	{
		const string events = "Splunk.Api receivers/simple test event";
		var options = new ReceiverOptions { Index = "main", Sourcetype = "splunk_api_it", Source = "integration-test", Host = "splunk-api-it" };

		var result = await Client.Receivers.SendAsync(events, options, Ct);

		result.Bytes.Should().Be(events.Length);
		result.Index.Should().Be("main");
		result.Sourcetype.Should().Be("splunk_api_it");
		result.Host.Should().Be("splunk-api-it");
		result.Source.Should().Be("integration-test");

		using var stream = new MemoryStream(Encoding.UTF8.GetBytes("Splunk.Api receivers/stream line one\nline two\n"));
		await Client.Receivers.SendStreamAsync(stream, options, Ct);
	}

	[Fact]
	public async Task Oneshot_QueuesTheFile()
	{
		var feed = await Client.OneshotInputs.CreateAsync(new OneshotInputCreateRequest { Name = VersionFile, Index = "main", Sourcetype = "splunk_api_it_oneshot" }, Ct);

		var queued = feed.Entries.Should().ContainSingle().Subject;
		queued.Name.Should().Be(VersionFile);
		queued.Content!.Size.Should().BePositive();
		try
		{
			// A small file finishes at once, after which Splunk no longer lists it.
			(await Client.OneshotInputs.GetAsync(VersionFile, Ct)).Entries.Should().ContainSingle();
		}
		catch (SplunkApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
		{
			(await Client.OneshotInputs.ListAsync(null, Ct)).Entries.Should().NotContain(i => i.Name == VersionFile);
		}
	}

	[Fact]
	public async Task ScriptedInput_RoundTrip()
	{
		const string script = "$SPLUNK_HOME/bin/scripts/readme.txt";
		var inputs = Client.ScriptedInputs;
		try
		{
			(await inputs.CreateAsync(new ScriptedInputCreateRequest { Name = script, Interval = "3600", Disabled = true, Sourcetype = "splunk_api_it" }, Ct))
				.Entries.Should().BeEmpty("Splunk 10.6 answers a create with an empty feed");

			await inputs.UpdateAsync(script, new ScriptedInputUpdateRequest { Interval = "0 3 * * *", Disabled = true }, Ct);
			await inputs.RestartAsync(new ScriptRestartRequest { Script = script }, Ct);

			var read = (await inputs.GetAsync(script, Ct)).Entries.Should().ContainSingle().Subject.Content!;
			read.Interval.Should().Be("0 3 * * *");
			read.Sourcetype.Should().Be("splunk_api_it");
		}
		finally
		{
			await inputs.DeleteAsync(script, Ct);
		}
	}

	[Fact]
	public async Task IngestDestination_RoundTrip()
	{
		var name = SplunkFixture.UniqueName("s3");
		var destinations = Client.IngestDestinations;
		try
		{
			var created = await destinations.CreateAsync(
				new IngestDestinationCreateRequest
				{
					Name = name,
					Path = "s3://example-bucket/splunk-api-it/",
					Endpoint = "https://s3.example.invalid",
					AccessKey = "AKIAEXAMPLE",
					SecretKey = "example-secret",
					Description = "Splunk.Api test",
					Compression = "gzip"
				},
				Ct);
			var destination = created.Entries.Should().ContainSingle().Subject.Content!;
			destination.SecretKey.Should().Be("<hidden>");
			destination.Compression.Should().Be("gzip");

			(await destinations.ListAsync(null, Ct)).Entries.Should().Contain(e => e.Name == name);
		}
		finally
		{
			await destinations.DeleteAsync(name, Ct);
		}

		(await destinations.ListAsync(null, Ct)).Entries.Should().NotContain(e => e.Name == name);
	}

	[Fact]
	public async Task IngestRuleset_RoundTrip()
	{
		var name = SplunkFixture.UniqueName("ruleset");
		var sourcetype = SplunkFixture.UniqueName("st");
		var rule = new IngestRule { Name = "drop_debug", Action = "filter", Condition = new IngestRuleCondition { Type = "regex", Field = "_raw", Match = "DEBUG" } };
		var rulesets = Client.IngestRulesets;
		try
		{
			var created = await rulesets.CreateAsync(new IngestRulesetCreateRequest { Name = name, Sourcetype = sourcetype, Description = "v1", Rules = [rule] }, Ct);
			created.Entries.Should().ContainSingle().Which.Content!.Rules.Should().ContainSingle().Which.Condition!.Match.Should().Be("DEBUG");

			await rulesets.UpdateAsync(name, new IngestRulesetUpdateRequest { Sourcetype = sourcetype, Description = "v2", Rules = [rule] }, Ct);

			(await rulesets.GetAsync(name, Ct)).Entries.Single().Content!.Description.Should().Be("v2");
			(await rulesets.ListAsync(Ct)).Entries.Should().Contain(e => e.Name == name);
		}
		finally
		{
			await fixture.DeleteUndocumentedAsync("services/data/ingest/rulesets/" + name, Ct);
		}
	}

	[Fact]
	public async Task IndexingPreview_CreateGetAndList()
	{
		var created = await Client.IndexingPreviews.CreateAsync(
			new IndexingPreviewCreateRequest { InputPath = VersionFile, AdditionalParameters = { ["props.SHOULD_LINEMERGE"] = "false" } },
			Ct);
		var jobId = created.Messages.Should().ContainSingle().Subject.Text;

		var preview = (await Client.IndexingPreviews.GetAsync(jobId, Ct)).Entries.Should().ContainSingle().Subject.Content!;

		preview.Explicit["SHOULD_LINEMERGE"].Value.Should().Be("false");
		preview.Inherited.Should().NotBeEmpty();
		(await Client.IndexingPreviews.ListAsync(Ct)).Entries.Should().Contain(e => e.Name == jobId);
	}

	[Fact]
	public async Task TcpSslSettings_Update()
	{
		var feed = await Client.TcpSslSettings.UpdateAsync("ssl", new TcpSslSettingsUpdateRequest { RequireClientCert = false }, Ct);

		feed.Generator.Should().NotBeNull();
		(await Client.TcpSslSettings.GetAsync("ssl", Ct)).Entries.Should().ContainSingle();
	}

	[Fact]
	public async Task HecSettings_UpdateKeepsTheCollectorRunning()
	{
		var current = (await Client.HecTokens.GetSettingsAsync(HecSettingsUpdateRequest.SettingsName, Ct)).Entries.Single().Content!;

		var updated = await Client.HecTokens.UpdateSettingsAsync(
			HecSettingsUpdateRequest.SettingsName,
			new HecSettingsUpdateRequest { Disabled = false, DedicatedIoThreads = current.DedicatedIoThreads },
			Ct);

		updated.Entries.Single().Content!.Disabled.Should().BeFalse();
		current.EnableSsl.Should().BeTrue();
	}

	[Fact]
	public async Task ReadsOnTheCollectorInstance_Succeed()
	{
		(await Client.MonitorInputs.ListAsync(new ListOptions { Count = 1 }, Ct)).Entries.Should().ContainSingle();
		(await Client.HecTokens.ListAsync(null, Ct)).Entries.Should().Contain(e => e.Name == "http://splunk_hec_token");
	}
}
