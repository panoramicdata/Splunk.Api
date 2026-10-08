using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SplunkHecClientTests
{
	[Fact]
	public async Task SendAsync_PostsTheEventsToTheEventEndpoint()
	{
		var stub = HecTestKit.Stub(HecTestKit.SuccessJson, HttpStatusCode.OK);
		using var client = HecTestKit.Create(stub, null);

		var reply = await client.SendAsync([new HecEvent { Event = "hello" }], TestContext.Current.CancellationToken);

		reply.AckId.Should().Be(3);
		stub.Calls[0].ShouldBeHec(HttpMethod.Post, "/services/collector/event", string.Empty, "{\"event\":\"hello\"}");
		stub.Calls[0].Headers.Contains("X-Splunk-Request-Channel").Should().BeFalse("no default channel is configured");
	}

	[Fact]
	public async Task SendRawAsync_PostsTheTextOnTheDefaultChannel()
	{
		var stub = HecTestKit.Stub(HecTestKit.SuccessJson, HttpStatusCode.OK);
		using var client = HecTestKit.Create(stub, HecTestKit.Channel);

		await client.SendRawAsync("raw line", TestContext.Current.CancellationToken);

		stub.Calls[0].ShouldBeHec(HttpMethod.Post, "/services/collector/raw", string.Empty, "raw line");
		stub.Calls[0].Headers.GetValues("X-Splunk-Request-Channel").Should().Equal(HecTestKit.Channel);
	}

	[Fact]
	public async Task QueryAcksAsync_ReturnsTheStatusOfEachId()
	{
		var stub = HecTestKit.Stub("""{"acks":{"3":true}}""", HttpStatusCode.OK);
		using var client = HecTestKit.Create(stub, HecTestKit.Channel);

		var acks = await client.QueryAcksAsync([3], TestContext.Current.CancellationToken);

		acks.Should().ContainSingle().Which.Should().Be(new KeyValuePair<long, bool>(3, true));
		stub.Calls[0].ShouldBeHec(HttpMethod.Post, "/services/collector/ack", string.Empty, "{\"acks\":[3]}");
	}

	[Fact]
	public async Task ServerBusy_IsRetried()
	{
		var stub = new StubHandler();
		stub.Enqueue(HttpStatusCode.ServiceUnavailable, """{"text":"Server is busy","code":9}""");
		stub.Enqueue(HttpStatusCode.OK, HecTestKit.SuccessJson);
		using var client = new SplunkHecClient(
			new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl + "/", Token = HecTestKit.Token, RetryBaseDelay = TimeSpan.Zero },
			stub);

		var reply = await client.SendAsync([new HecEvent { Event = "x" }], TestContext.Current.CancellationToken);

		reply.Code.Should().Be(0);
		stub.Calls.Should().HaveCount(2);
	}

	[Theory]
	[InlineData("https://splunk.test:8088", "https://splunk.test:8088/")]
	[InlineData("http://lb.example.com/hec/", "http://lb.example.com/hec/")]
	public void BaseAddress_EndsWithASlash(string baseUrl, string expected)
	{
		using var client = new SplunkHecClient(new SplunkHecClientOptions { BaseUrl = baseUrl, Token = HecTestKit.Token }, new StubHandler());

		client.BaseAddress.Should().Be(new Uri(expected));
	}

	[Fact]
	public void Constructor_WithTransportSettings_CreatesAClient()
	{
		using var pinned = new SplunkHecClient(new SplunkHecClientOptions
		{
			BaseUrl = HecTestKit.BaseUrl,
			Token = HecTestKit.Token,
			TrustedServerCertificateThumbprint = new string('A', 64)
		});
		using var custom = new SplunkHecClient(new SplunkHecClientOptions
		{
			BaseUrl = HecTestKit.BaseUrl,
			Token = HecTestKit.Token,
			ServerCertificateValidationCallback = (_, _, _) => true
		});
		using var plain = new SplunkHecClient(new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl, Token = HecTestKit.Token });

		pinned.Collector.Should().NotBeNull();
		custom.Collector.Should().NotBeNull();
		plain.Collector.Should().NotBeNull();
	}

	[Fact]
	public void SplunkClient_WithTransportSettings_SharesTheTransportFactory()
	{
		using var client = new SplunkClient(new SplunkClientOptions
		{
			BaseUrl = "https://splunk.test:8089",
			Token = "token",
			TrustedServerCertificateThumbprint = new string('b', 64)
		});

		client.BaseAddress.Should().Be(new Uri("https://splunk.test:8089/"));
	}

	[Fact]
	public void ContentSerializer_NamesFieldsAsJson()
	{
		var serializer = new Serialization.HecContentSerializer();

		serializer.GetFieldNameForProperty(typeof(HecEvent).GetProperty(nameof(HecEvent.Sourcetype))!).Should().Be("sourcetype");
	}

	[Fact]
	public void Constructor_RejectsNulls()
	{
		var withoutOptions = () => new SplunkHecClient(null!);
		var withoutOptionsButHandler = () => new SplunkHecClient(null!, new StubHandler());
		var withoutHandler = () => new SplunkHecClient(new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl, Token = HecTestKit.Token }, null!);

		withoutOptions.Should().Throw<ArgumentNullException>();
		withoutOptionsButHandler.Should().Throw<ArgumentNullException>();
		withoutHandler.Should().Throw<ArgumentNullException>();
	}

	public static TheoryData<SplunkHecClientOptions, string> InvalidOptions => new()
	{
		{ new SplunkHecClientOptions { BaseUrl = "not a url", Token = HecTestKit.Token }, "BaseUrl" },
		{ new SplunkHecClientOptions { BaseUrl = "ftp://splunk.test", Token = HecTestKit.Token }, "BaseUrl" },
		{ new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl, Token = " " }, "Token" },
		{ new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl, Token = HecTestKit.Token, Channel = "not-a-guid" }, "Channel" },
		{ new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl, Token = HecTestKit.Token, TrustedServerCertificateThumbprint = "ABC" }, "thumbprint" },
		{ new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl, Token = HecTestKit.Token, MaxRetries = -1 }, "MaxRetries" },
		{ new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl, Token = HecTestKit.Token, Timeout = TimeSpan.Zero }, "Timeout" },
		{ new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl, Token = HecTestKit.Token, RetryBaseDelay = TimeSpan.FromSeconds(-1) }, "RetryBaseDelay" },
		{ new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl, Token = HecTestKit.Token, MaxRetryDelay = TimeSpan.Zero }, "MaxRetryDelay" }
	};

	[Theory]
	[MemberData(nameof(InvalidOptions))]
	public void Constructor_RejectsInvalidOptions(SplunkHecClientOptions options, string parameter)
	{
		var act = () => new SplunkHecClient(options, new StubHandler());

		act.Should().Throw<ArgumentException>().Which.ParamName.Should().Be(parameter);
	}

	[Fact]
	public void Options_ToString_MasksTheToken()
	{
		new SplunkHecClientOptions { BaseUrl = HecTestKit.BaseUrl, Token = HecTestKit.Token, Channel = HecTestKit.Channel }.ToString()
			.Should().Be($"SplunkHecClientOptions {{ BaseUrl = {HecTestKit.BaseUrl}, Token = ***, Channel = {HecTestKit.Channel} }}");
		new SplunkHecClientOptions().ToString()
			.Should().Be("SplunkHecClientOptions { BaseUrl = , Token = (none), Channel = (none) }");
	}

	[Fact]
	public void Dispose_CanBeCalledTwice()
	{
		var client = HecTestKit.Create(new StubHandler(), null);

		client.Dispose();
		var again = client.Dispose;

		again.Should().NotThrow();
	}
}
