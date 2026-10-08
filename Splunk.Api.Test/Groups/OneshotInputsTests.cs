using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class OneshotInputsTests
{
	private const string Path = "/services/data/inputs/oneshot";

	// Captured from Splunk 10.6.0.5: the reply to queueing a file.
	private static readonly string OneshotJson = InputsTestKit.Feed("/opt/splunk/etc/splunk.version", """
		{ "Bytes Indexed": 12, "Offset": 40, "Size": 73, "Sources Indexed": 0, "Spool Time": "Thu Oct  8 14:02:38 UTC 2026", "eai:acl": null }
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.OneshotInputs.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.OneshotInputs.CreateAsync(
			new OneshotInputCreateRequest
			{
				Name = "/tmp/app.log",
				Host = "web01",
				HostRegex = "x",
				HostSegment = 2,
				Index = "main",
				RenameSource = "app",
				Sourcetype = "app_log"
			},
			ct)))
			.ShouldBePost(Path, "name=%2Ftmp%2Fapp.log&host=web01&host_regex=x&host_segment=2&index=main&rename-source=app&sourcetype=app_log");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.OneshotInputs.GetAsync("/tmp/app.log", ct))).ShouldBeGet(Path + "/%2Ftmp%2Fapp.log");

	[Fact]
	public async Task CreateAsync_MapsEveryModelledField()
	{
		var entry = await InputsTestKit.MapEntryAsync(
			(c, ct) => c.OneshotInputs.CreateAsync(new OneshotInputCreateRequest { Name = "/opt/splunk/etc/splunk.version" }, ct),
			OneshotJson);

		entry.Name.Should().Be("/opt/splunk/etc/splunk.version");
		var input = entry.Content!;
		input.BytesIndexed.Should().Be(12);
		input.Offset.Should().Be(40);
		input.Size.Should().Be(73);
		input.SourcesIndexed.Should().Be(0);
		input.SpoolTime.Should().Be("Thu Oct  8 14:02:38 UTC 2026");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.OneshotInputs.GetAsync("/nope", ct));
}
