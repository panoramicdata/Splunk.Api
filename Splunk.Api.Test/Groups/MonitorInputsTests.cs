using Splunk.Api.Models;
using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class MonitorInputsTests
{
	private const string Path = "/services/data/inputs/monitor";
	private const string Item = Path + "/%2Fvar%2Flog%2Fapp";

	// Captured from Splunk 10.6.0.5 after creating a monitor input; host names replaced.
	private static readonly string MonitorJson = InputsTestKit.Feed("/var/log/app", """
		{
			"_TCP_ROUTING": "*", "_rcvbuf": 1572864, "blacklist": "\\.gz$", "crcSalt": "<SOURCE>", "disabled": true,
			"eai:acl": null, "filecount": 3, "filestatecount": 2, "followTail": false, "host": "$decideOnStartup",
			"host_regex": "^/var/(\\w+)", "host_resolved": "splunk01", "host_segment": 3, "ignoreOlderThan": "7d",
			"index": "main", "recursive": false, "source": "app", "sourcetype": "app_log", "time_before_close": 3,
			"whitelist": "\\.log$"
		}
		""");

	private static readonly string MembersJson = InputsTestKit.Feed("/opt/splunk/var/log/splunk/audit.log", """{ "eai:acl": null }""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.MonitorInputs.ListAsync(new ListOptions { Count = 0, Search = "main" }, ct)))
			.ShouldBe(HttpMethod.Get, Path, "?count=0&search=main&output_mode=json", null);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.MonitorInputs.CreateAsync(
			new MonitorInputCreateRequest
			{
				Name = "/var/log/app",
				Blacklist = "gz$",
				CheckIndex = true,
				CheckPath = false,
				CrcSalt = "<SOURCE>",
				Disabled = true,
				FollowTail = false,
				Host = "web01",
				HostRegex = "x",
				HostSegment = 3,
				IgnoreOlderThan = "7d",
				Index = "main",
				Recursive = false,
				RenameSource = "app",
				Sourcetype = "app_log",
				TimeBeforeClose = 3,
				Whitelist = "log$"
			},
			ct)))
			.ShouldBePost(Path, "name=%2Fvar%2Flog%2Fapp&blacklist=gz%24&check-index=true&check-path=false&crc-salt=%3CSOURCE%3E&disabled=true&followTail=false&host=web01&host_regex=x&host_segment=3&ignore-older-than=7d&index=main&recursive=false&rename-source=app&sourcetype=app_log&time-before-close=3&whitelist=log%24");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.MonitorInputs.GetAsync("/var/log/app", ct))).ShouldBeGet(Item);

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.MonitorInputs.UpdateAsync("/var/log/app", new MonitorInputUpdateRequest { Disabled = false }, ct)))
			.ShouldBePost(Item, "disabled=false");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.MonitorInputs.DeleteAsync("/var/log/app", ct))).ShouldBeDelete(Item);

	[Fact]
	public async Task ListMembersAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.MonitorInputs.ListMembersAsync("/var/log/app", new ListOptions { Count = 2 }, ct)))
			.ShouldBe(HttpMethod.Get, Item + "/members", "?count=2&output_mode=json", null);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await InputsTestKit.MapEntryAsync((c, ct) => c.MonitorInputs.GetAsync("/var/log/app", ct), MonitorJson);

		entry.Name.Should().Be("/var/log/app");
		var input = entry.Content!;
		input.TcpRouting.Should().Be("*");
		input.ReceiveBufferSize.Should().Be(1572864);
		input.Blacklist.Should().Be("\\.gz$");
		input.Whitelist.Should().Be("\\.log$");
		input.CrcSalt.Should().Be("<SOURCE>");
		input.Disabled.Should().BeTrue();
		input.FileCount.Should().Be(3);
		input.FileStateCount.Should().Be(2);
		input.FollowTail.Should().BeFalse();
		input.Host.Should().Be("$decideOnStartup");
		input.HostRegex.Should().Be("^/var/(\\w+)");
		input.HostResolved.Should().Be("splunk01");
		input.HostSegment.Should().Be(3);
		input.IgnoreOlderThan.Should().Be("7d");
		input.Index.Should().Be("main");
		input.Recursive.Should().BeFalse();
		input.Source.Should().Be("app");
		input.Sourcetype.Should().Be("app_log");
		input.TimeBeforeClose.Should().Be(3);
	}

	[Fact]
	public async Task ListMembersAsync_MapsTheFiles()
	{
		var entry = await InputsTestKit.MapEntryAsync((c, ct) => c.MonitorInputs.ListMembersAsync("$SPLUNK_HOME/var/log/splunk", null, ct), MembersJson);

		entry.Name.Should().Be("/opt/splunk/var/log/splunk/audit.log");
		entry.Content!.AdditionalProperties.Should().BeEmpty();
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.MonitorInputs.GetAsync("/nope", ct));
}
