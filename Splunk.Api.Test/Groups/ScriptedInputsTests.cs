using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class ScriptedInputsTests
{
	private const string Path = "/services/data/inputs/script";
	private const string Item = Path + "/%24SPLUNK_HOME%2Fbin%2Fscripts%2Fapp.sh";
	private const string Script = "$SPLUNK_HOME/bin/scripts/app.sh";

	// Captured from Splunk 10.6.0.5; host names replaced.
	private static readonly string ScriptJson = InputsTestKit.Feed(Script, """
		{
			"_rcvbuf": 1572864, "disabled": true, "eai:acl": null, "endtime": "1791468000", "group": "exec commands",
			"host": "$decideOnStartup", "host_resolved": "splunk01", "index": "default", "interval": "0 */4 * * *",
			"passAuth": "splunk-system-user", "python.version": null, "source": "app", "sourcetype": "script",
			"starttime": "1791467000"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ScriptedInputs.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ScriptedInputs.CreateAsync(
			new ScriptedInputCreateRequest
			{
				Name = Script,
				Interval = "60",
				Disabled = true,
				Host = "web01",
				Index = "main",
				PassAuth = "admin",
				RenameSource = "app",
				Source = "src",
				Sourcetype = "app_script"
			},
			ct)))
			.ShouldBePost(Path, "name=%24SPLUNK_HOME%2Fbin%2Fscripts%2Fapp.sh&interval=60&disabled=true&host=web01&index=main&passAuth=admin&rename-source=app&source=src&sourcetype=app_script");

	[Fact]
	public async Task RestartAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ScriptedInputs.RestartAsync(new ScriptRestartRequest { Script = Script }, ct)))
			.ShouldBePost(Path + "/restart", "script=%24SPLUNK_HOME%2Fbin%2Fscripts%2Fapp.sh");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ScriptedInputs.GetAsync(Script, ct))).ShouldBeGet(Item);

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ScriptedInputs.UpdateAsync(
			Script,
			new ScriptedInputUpdateRequest
			{
				Disabled = false,
				Host = "web02",
				Index = "summary",
				Interval = "0 * * * *",
				PassAuth = "nobody",
				RenameSource = "app2",
				Source = "src2",
				Sourcetype = "app_script2"
			},
			ct)))
			.ShouldBePost(Item, "disabled=false&host=web02&index=summary&interval=0+%2A+%2A+%2A+%2A&passAuth=nobody&rename-source=app2&source=src2&sourcetype=app_script2");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ScriptedInputs.DeleteAsync(Script, ct))).ShouldBeDelete(Item);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var entry = await InputsTestKit.MapEntryAsync((c, ct) => c.ScriptedInputs.GetAsync(Script, ct), ScriptJson);

		var input = entry.Content!;
		input.Disabled.Should().BeTrue();
		input.EndTime.Should().Be("1791468000");
		input.StartTime.Should().Be("1791467000");
		input.Group.Should().Be("exec commands");
		input.Interval.Should().Be("0 */4 * * *");
		input.PassAuth.Should().Be("splunk-system-user");
		input.Source.Should().Be("app");
		input.Sourcetype.Should().Be("script");
		input.Index.Should().Be("default");
		input.AdditionalProperties.Should().ContainKey("python.version");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.ScriptedInputs.GetAsync("nope", ct));
}
