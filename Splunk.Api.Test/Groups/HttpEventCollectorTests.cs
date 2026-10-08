using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;
using System.Net;
using System.Text;

namespace Splunk.Api.Test.Groups;

public class HttpEventCollectorTests
{
	private static readonly HecEvent[] Events =
	[
		new HecEvent { Event = "hello" },
		new HecEvent
		{
			Event = new Dictionary<string, object> { ["action"] = "login", ["user"] = "alice" },
			Time = DateTimeOffset.FromUnixTimeMilliseconds(1791468575123),
			Host = "web01",
			Source = "app",
			Sourcetype = "app_json",
			Index = "main",
			Fields = new Dictionary<string, object> { ["region"] = "eu", ["shard"] = 3 }
		}
	];

	private const string EventsBody = "{\"event\":\"hello\"}\n{\"event\":{\"action\":\"login\",\"user\":\"alice\"},\"time\":1791468575.123,\"host\":\"web01\",\"source\":\"app\",\"sourcetype\":\"app_json\",\"index\":\"main\",\"fields\":{\"region\":\"eu\",\"shard\":3}}";

	private static readonly HecRequestOptions Options = new()
	{
		Channel = HecTestKit.Channel,
		Host = "web01",
		Index = "main",
		Source = "app",
		Sourcetype = "app_log",
		Time = 1791468575,
		AutoExtractTimestamp = true
	};

	private const string OptionsQuery = "?host=web01&index=main&source=app&sourcetype=app_log&time=1791468575&auto_extract_timestamp=true&channel=" + HecTestKit.Channel;

	[Fact]
	public async Task SendAsync_SendsExactRequest()
	{
		var call = await HecTestKit.CaptureAsync((c, ct) => c.Collector.SendAsync(Events, null, ct), HecTestKit.SuccessJson);

		call.ShouldBeHec(HttpMethod.Post, "/services/collector", string.Empty, EventsBody);
		call.ContentType.Should().Be("application/json");
		call.Headers.GetValues("X-Splunk-Request-Channel").Should().Equal(HecTestKit.Channel);
	}

	[Fact]
	public async Task SendEventsAsync_SendsExactRequest()
		=> (await HecTestKit.CaptureAsync((c, ct) => c.Collector.SendEventsAsync(Events, Options, ct), HecTestKit.SuccessJson))
			.ShouldBeHec(HttpMethod.Post, "/services/collector/event", OptionsQuery, EventsBody);

	[Fact]
	public async Task SendEventsV1Async_SendsExactRequest()
		=> (await HecTestKit.CaptureAsync((c, ct) => c.Collector.SendEventsV1Async(Events[..1], null, ct), HecTestKit.SuccessJson))
			.ShouldBeHec(HttpMethod.Post, "/services/collector/event/1.0", string.Empty, "{\"event\":\"hello\"}");

	[Fact]
	public async Task SendRawAsync_SendsExactRequest()
	{
		var call = await HecTestKit.CaptureAsync((c, ct) => c.Collector.SendRawAsync("line one\nline two", Options, ct), HecTestKit.SuccessJson);

		call.ShouldBeHec(HttpMethod.Post, "/services/collector/raw", OptionsQuery, "line one\nline two");
		call.ContentType.Should().Be("text/plain");
		call.Headers.Contains("X-Splunk-Request-Channel").Should().BeFalse("the query names the channel");
	}

	[Fact]
	public async Task SendRawV1Async_SendsExactRequest()
		=> (await HecTestKit.CaptureAsync((c, ct) => c.Collector.SendRawV1Async("raw", null, ct), HecTestKit.SuccessJson))
			.ShouldBeHec(HttpMethod.Post, "/services/collector/raw/1.0", string.Empty, "raw");

	[Fact]
	public async Task SendMintAsync_SendsExactRequest()
		=> (await HecTestKit.CaptureAsync((c, ct) => c.Collector.SendMintAsync("{\"mint\":1}", new HecRequestOptions { Index = "mint" }, ct), HecTestKit.SuccessJson))
			.ShouldBeHec(HttpMethod.Post, "/services/collector/mint", "?index=mint", "{\"mint\":1}");

	[Fact]
	public async Task SendMintV1Async_SendsExactRequest()
		=> (await HecTestKit.CaptureAsync((c, ct) => c.Collector.SendMintV1Async("{\"mint\":1}", null, ct), HecTestKit.SuccessJson))
			.ShouldBeHec(HttpMethod.Post, "/services/collector/mint/1.0", string.Empty, "{\"mint\":1}");

	[Fact]
	public async Task QueryAcksAsync_SendsExactRequest()
	{
		var call = await HecTestKit.CaptureAsync(
			(c, ct) => c.Collector.QueryAcksAsync(new HecAckRequest { Acks = [0, 1] }, new HecChannelOptions { Channel = HecTestKit.Channel }, ct),
			"""{"acks":{"0":true,"1":false}}""");

		call.ShouldBeHec(HttpMethod.Post, "/services/collector/ack", "?channel=" + HecTestKit.Channel, "{\"acks\":[0,1]}");
		call.ContentType.Should().Be("application/json");
	}

	[Fact]
	public async Task GetHealthAsync_SendsExactRequest()
		=> (await HecTestKit.CaptureAsync((c, ct) => c.Collector.GetHealthAsync(new HecHealthOptions { Ack = true, Token = "abc" }, ct), """{"text":"HEC is healthy","code":17}"""))
			.ShouldBeHec(HttpMethod.Get, "/services/collector/health", "?ack=true&token=abc", null);

	[Fact]
	public async Task GetHealthV1Async_SendsExactRequest()
		=> (await HecTestKit.CaptureAsync((c, ct) => c.Collector.GetHealthV1Async(null, ct), """{"text":"HEC is healthy","code":17}"""))
			.ShouldBeHec(HttpMethod.Get, "/services/collector/health/1.0", string.Empty, null);

	[Fact]
	public async Task SendS2SAsync_SendsExactRequest()
	{
		using var data = new MemoryStream(Encoding.ASCII.GetBytes("--splunk-cooked-mode-v3--"));

		var call = await HecTestKit.CaptureAsync((c, ct) => c.Collector.SendS2SAsync(data, ct), """{"text":"Success","code":0}""");

		call.ShouldBeHec(HttpMethod.Post, "/services/collector/s2s", string.Empty, "--splunk-cooked-mode-v3--");
	}

	[Fact]
	public async Task Responses_MapEveryModelledField()
	{
		using var client = HecTestKit.Create(HecTestKit.Stub("""{"text":"Event field is required","code":12,"invalid-event-number":1,"ackId":6}""", HttpStatusCode.OK), null);

		var reply = await client.Collector.SendEventsAsync(Events, null, TestContext.Current.CancellationToken);

		reply.Text.Should().Be("Event field is required");
		reply.Code.Should().Be(12);
		reply.InvalidEventNumber.Should().Be(1);
		reply.AckId.Should().Be(6);
	}

	[Fact]
	public async Task QueryAcksAsync_MapsEveryModelledField()
	{
		using var client = HecTestKit.Create(HecTestKit.Stub("""{"acks":{"0":true,"1":false}}""", HttpStatusCode.OK), HecTestKit.Channel);

		var reply = await client.Collector.QueryAcksAsync(new HecAckRequest { Acks = [0, 1] }, null, TestContext.Current.CancellationToken);

		reply.Acks.Should().BeEquivalentTo(new Dictionary<long, bool> { [0] = true, [1] = false });
	}

	[Fact]
	public async Task Rejection_RaisesSplunkHecException()
	{
		using var client = HecTestKit.Create(HecTestKit.Stub("""{"text":"Invalid data format","code":6,"invalid-event-number":0}""", HttpStatusCode.BadRequest), null);

		var act = () => client.Collector.SendEventsAsync(Events, null, TestContext.Current.CancellationToken);

		var thrown = (await act.Should().ThrowAsync<SplunkHecException>()).Which;
		thrown.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		thrown.Message.Should().Be("Invalid data format");
		thrown.Code.Should().Be(6);
		thrown.InvalidEventNumber.Should().Be(0);
	}

	[Theory]
	[InlineData("<html>busy</html>")]
	[InlineData("""{"code":9}""")]
	public async Task RejectionWithoutText_FallsBackToTheStatus(string body)
	{
		using var client = HecTestKit.Create(HecTestKit.Stub(body, HttpStatusCode.ServiceUnavailable), null);

		var act = () => client.Collector.GetHealthAsync(null, TestContext.Current.CancellationToken);

		(await act.Should().ThrowAsync<SplunkHecException>()).Which.Message.Should().Be("HTTP 503 (Service Unavailable)");
	}
}
