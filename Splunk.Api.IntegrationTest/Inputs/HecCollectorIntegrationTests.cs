using Splunk.Api.Models.Inputs;
using System.Net;
using System.Text;

namespace Splunk.Api.IntegrationTest.Inputs;

/// <summary>Sends data through the HTTP Event Collector of the collector test container.</summary>
[Collection(SplunkHecTestGroup.Name)]
public class HecCollectorIntegrationTests(SplunkHecFixture fixture)
{
	private const string Sourcetype = "splunk_api_it";

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private static HecEvent[] Events(string marker) =>
	[
		new HecEvent { Event = "Splunk.Api integration test " + marker, Sourcetype = Sourcetype, Index = "main" },
		new HecEvent
		{
			Event = new Dictionary<string, object> { ["marker"] = marker, ["n"] = 2 },
			Time = DateTimeOffset.UtcNow,
			Host = "splunk-api-it",
			Source = "integration-test",
			Sourcetype = Sourcetype,
			Fields = new Dictionary<string, object> { ["it_marker"] = marker }
		}
	];

	[Fact]
	public async Task Health_IsHealthy()
	{
		(await fixture.Hec.Collector.GetHealthAsync(null, Ct)).Code.Should().Be(17);
		(await fixture.Hec.Collector.GetHealthAsync(new HecHealthOptions { Ack = true, Token = fixture.Token }, Ct)).Text.Should().Be("HEC is healthy");
		(await fixture.Hec.Collector.GetHealthV1Async(null, Ct)).Code.Should().Be(17);
	}

	[Fact]
	public async Task Events_AreAcceptedOnEveryEventPath()
	{
		var marker = Guid.NewGuid().ToString("N");

		(await fixture.Hec.SendAsync(Events(marker), Ct)).Text.Should().Be("Success");
		(await fixture.Hec.Collector.SendAsync(Events(marker), new HecRequestOptions { Index = "main" }, Ct)).Code.Should().Be(0);
		(await fixture.Hec.Collector.SendEventsAsync(Events(marker), new HecRequestOptions { AutoExtractTimestamp = true }, Ct)).Code.Should().Be(0);
		(await fixture.Hec.Collector.SendEventsV1Async(Events(marker), null, Ct)).Code.Should().Be(0);
	}

	[Fact]
	public async Task RawData_IsAcceptedOnBothRawPaths()
	{
		var options = new HecRequestOptions { Channel = Guid.NewGuid().ToString(), Sourcetype = Sourcetype, Host = "splunk-api-it", Time = DateTimeOffset.UtcNow.ToUnixTimeSeconds() };

		(await fixture.Hec.SendRawAsync("raw line one\nraw line two", Ct)).Code.Should().Be(0);
		(await fixture.Hec.Collector.SendRawAsync("raw line three", options, Ct)).Code.Should().Be(0);
		(await fixture.Hec.Collector.SendRawV1Async("raw line four", options, Ct)).Code.Should().Be(0);
	}

	[Fact]
	public async Task Mint_WithoutAMintPayload_AnswersNoData()
	{
		var options = new HecRequestOptions { Channel = Guid.NewGuid().ToString() };

		var mint = () => fixture.Hec.Collector.SendMintAsync("{\"not\":\"mint\"}", options, Ct);
		var mintV1 = () => fixture.Hec.Collector.SendMintV1Async("{\"not\":\"mint\"}", options, Ct);

		(await mint.Should().ThrowAsync<SplunkHecException>()).Which.Code.Should().Be(5);
		(await mintV1.Should().ThrowAsync<SplunkHecException>()).Which.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task S2S_WithoutAnS2SPayload_IsRejected()
	{
		using var data = new MemoryStream(Encoding.ASCII.GetBytes("not really s2s"));

		var act = () => fixture.Hec.Collector.SendS2SAsync(data, Ct);

		(await act.Should().ThrowAsync<SplunkHecException>()).Which.Message.Should().Be("Invalid data format");
	}

	[Fact]
	public async Task InvalidToken_RaisesSplunkHecException()
	{
		using var client = new SplunkHecClient(fixture.CreateHecOptions(o => o.Token = Guid.NewGuid().ToString()));

		var act = () => client.SendAsync(Events("bad-token"), Ct);

		var thrown = (await act.Should().ThrowAsync<SplunkHecException>()).Which;
		thrown.StatusCode.Should().Be(HttpStatusCode.Forbidden);
		thrown.Code.Should().Be(4);
		thrown.Message.Should().Be("Invalid token");
	}

	[Fact]
	public async Task AcknowledgedEvents_AreReportedIndexed()
	{
		var name = SplunkFixture.UniqueName("hec_ack");
		var tokens = fixture.Management.HecTokens;
		try
		{
			var token = (await tokens.CreateAsync(new HecTokenCreateRequest { Name = name, UseAck = true, Index = "main", Indexes = ["main"] }, Ct))
				.Entries.Single().Content!.Token!;
			using var client = new SplunkHecClient(fixture.CreateHecOptions(o =>
			{
				o.Token = token;
				o.Channel = Guid.NewGuid().ToString();
			}));

			var reply = await SendWhenTokenIsLiveAsync(client);
			reply.AckId.Should().NotBeNull();

			(await WaitForAckAsync(client, reply.AckId!.Value)).Should().BeTrue();
			var explicitChannel = await client.Collector.QueryAcksAsync(new HecAckRequest { Acks = [reply.AckId.Value] }, new HecChannelOptions { Channel = Guid.NewGuid().ToString() }, Ct);
			explicitChannel.Acks[reply.AckId.Value].Should().BeFalse("acknowledgements belong to the channel the events were sent on");
		}
		finally
		{
			await tokens.DeleteAsync(name, Ct);
		}
	}

	[Fact]
	public async Task Connections_ListTheSender()
	{
		await fixture.Hec.SendAsync(Events("connections"), Ct);

		var feed = await fixture.Management.HecConnections.ListAsync(Ct);

		var sender = feed.Entries.Should().NotBeEmpty().And.Subject.First();
		sender.Content!.IpAddress.Should().Be(sender.Name);
		var single = await fixture.Management.HecConnections.GetAsync(sender.Name, Ct);
		single.Entries.Should().ContainSingle().Which.Content!.LastConnectionTime.Should().NotBeNull();
	}

	/// <summary>A new token is live once the collector has reloaded; retry briefly until then.</summary>
	private static async Task<HecResponse> SendWhenTokenIsLiveAsync(SplunkHecClient client)
	{
		for (var attempt = 1; ; attempt++)
		{
			try
			{
				return await client.SendAsync(Events("ack"), Ct);
			}
			catch (SplunkHecException e) when (e.Code == 4 && attempt < 20)
			{
				await Task.Delay(TimeSpan.FromMilliseconds(500), Ct);
			}
		}
	}

	private static async Task<bool> WaitForAckAsync(SplunkHecClient client, long ackId)
	{
		for (var attempt = 0; attempt < 60; attempt++)
		{
			var acks = await client.QueryAcksAsync([ackId], Ct);
			if (acks.TryGetValue(ackId, out var indexed) && indexed)
			{
				return true;
			}

			await Task.Delay(TimeSpan.FromSeconds(1), Ct);
		}

		return false;
	}
}
