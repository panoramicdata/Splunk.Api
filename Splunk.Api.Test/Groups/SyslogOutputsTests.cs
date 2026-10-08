using Splunk.Api.Models.Outputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class SyslogOutputsTests
{
	private const string Path = "/services/data/outputs/tcp/syslog";

	// Captured from Splunk 10.6.0.5 after creating a syslog group at a documentation address.
	private static readonly string SyslogJson = InputsTestKit.Feed("siem", """
		{
			"disabled": true, "eai:acl": null, "priority": "13", "server": "192.0.2.1:514", "syslogSourceType": "sourcetype::app_log",
			"timestampformat": "%b %e %H:%M:%S", "type": "udp"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.SyslogOutputs.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.SyslogOutputs.CreateAsync(
			new SyslogOutputCreateRequest
			{
				Name = "siem",
				Disabled = true,
				Priority = 13,
				Server = "192.0.2.1:514",
				SyslogSourceType = "sourcetype::app_log",
				TimestampFormat = "%b %e",
				Type = "udp"
			},
			ct)))
			.ShouldBePost(Path, "name=siem&disabled=true&priority=13&server=192.0.2.1%3A514&syslogSourceType=sourcetype%3A%3Aapp_log&timestampformat=%25b+%25e&type=udp");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.SyslogOutputs.GetAsync("siem", ct))).ShouldBeGet(Path + "/siem");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.SyslogOutputs.UpdateAsync("siem", new SyslogOutputUpdateRequest { Type = "tcp" }, ct)))
			.ShouldBePost(Path + "/siem", "type=tcp");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.SyslogOutputs.DeleteAsync("siem", ct))).ShouldBeDelete(Path + "/siem");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var syslog = (await InputsTestKit.MapEntryAsync((c, ct) => c.SyslogOutputs.GetAsync("siem", ct), SyslogJson)).Content!;

		syslog.Disabled.Should().BeTrue();
		syslog.Priority.Should().Be(13);
		syslog.Server.Should().Be("192.0.2.1:514");
		syslog.SyslogSourceType.Should().Be("sourcetype::app_log");
		syslog.TimestampFormat.Should().Be("%b %e %H:%M:%S");
		syslog.Type.Should().Be("udp");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.SyslogOutputs.GetAsync("nope", ct));
}
