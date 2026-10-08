using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;
using System.Text;

namespace Splunk.Api.Test.Groups;

public class ReceiversTests
{
	// Captured from Splunk 10.6.0.5: the reply to receivers/simple.
	private const string ReceivedJson = """{"index":"main","bytes":16,"host":"web01","source":"app","sourcetype":"app_log"}""";

	private static readonly ReceiverOptions Options = new()
	{
		Host = "web01",
		HostRegex = "x",
		Index = "main",
		Source = "app",
		Sourcetype = "app_log"
	};

	[Fact]
	public async Task SendAsync_SendsExactRequest()
	{
		var call = await InputsTestKit.CaptureAsync((c, ct) => c.Receivers.SendAsync("simple event one", Options, ct), ReceivedJson);

		call.ShouldBe(HttpMethod.Post, "/services/receivers/simple", "?host=web01&host_regex=x&index=main&source=app&sourcetype=app_log&output_mode=json", "simple event one");
		call.ContentType.Should().Be("text/plain");
	}

	[Fact]
	public async Task SendStreamAsync_SendsExactRequest()
	{
		using var events = new MemoryStream(Encoding.UTF8.GetBytes("line one\nline two\n"));

		var call = await InputsTestKit.CaptureAsync((c, ct) => c.Receivers.SendStreamAsync(events, null, ct));

		call.Method.Should().Be(HttpMethod.Post);
		call.Uri.AbsolutePath.Should().Be("/services/receivers/stream");
		call.Uri.Query.Should().Be(InputsTestKit.JsonQuery);
		call.Body.Should().Be("line one\nline two\n");
		call.Headers.GetValues("x-splunk-input-mode").Should().Equal("streaming");
	}

	[Fact]
	public async Task SendAsync_MapsEveryModelledField()
	{
		var result = await InputsTestKit.MapAsync((c, ct) => c.Receivers.SendAsync("simple event one", null, ct), ReceivedJson);

		result.Index.Should().Be("main");
		result.Bytes.Should().Be(16);
		result.Host.Should().Be("web01");
		result.Source.Should().Be("app");
		result.Sourcetype.Should().Be("app_log");
	}

	[Fact]
	public Task SendAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.Receivers.SendAsync("x", null, ct));
}
