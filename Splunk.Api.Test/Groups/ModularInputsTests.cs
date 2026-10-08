using Splunk.Api.Models;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class ModularInputsTests
{
	private const string Path = "/services/data/modular-inputs";

	// Captured from Splunk 10.6.0.5, trimmed to two parameters.
	private static readonly string ModularJson = InputsTestKit.Feed("journald", """
		{
			"description": "This is the input that gets data from journald (systemd's logging component) into Splunk.",
			"eai:acl": null,
			"endpoint": {
				"args": {
					"journalctl-boot": { "data_type": "string", "description": "", "order": 7, "required_on_create": false, "required_on_edit": false, "title": "journalctl-boot" },
					"name": { "data_type": "string", "description": "", "order": 0, "title": "name" }
				}
			},
			"streaming_mode": "json", "title": "Systemd Journald Input for Splunk", "use_single_instance": false
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ModularInputs.ListAsync(new ListOptions { Offset = 10 }, ct)))
			.ShouldBe(HttpMethod.Get, Path, "?offset=10&output_mode=json", null);

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.ModularInputs.GetAsync("journald", ct))).ShouldBeGet(Path + "/journald");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var input = (await InputsTestKit.MapEntryAsync((c, ct) => c.ModularInputs.GetAsync("journald", ct), ModularJson)).Content!;

		input.Title.Should().Be("Systemd Journald Input for Splunk");
		input.Description.Should().StartWith("This is the input");
		input.StreamingMode.Should().Be("json");
		input.UseSingleInstance.Should().BeFalse();
		input.Endpoint!.Arguments.Should().HaveCount(2);
		var boot = input.Endpoint.Arguments["journalctl-boot"];
		boot.Title.Should().Be("journalctl-boot");
		boot.Description.Should().BeEmpty();
		boot.DataType.Should().Be("string");
		boot.Order.Should().Be(7);
		boot.RequiredOnCreate.Should().BeFalse();
		boot.RequiredOnEdit.Should().BeFalse();
		input.Endpoint.Arguments["name"].RequiredOnCreate.Should().BeNull();
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.ModularInputs.GetAsync("nope", ct));
}
