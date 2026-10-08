using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class RegistryInputsTests
{
	private const string Path = "/services/data/inputs/registry";

	// Shaped as the reference's example (a Linux test instance has no registry inputs).
	private static readonly string RegistryJson = InputsTestKit.Feed("hklm", """
		{
			"baseline": "0", "disabled": "1", "eai:acl": null, "hive": "HKLM", "index": "default", "monitorSubnodes": "1",
			"proc": "c:\\.*", "type": ["set", "create", "delete", "rename"]
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RegistryInputs.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RegistryInputs.CreateAsync(
			new RegistryInputCreateRequest
			{
				Name = "hklm",
				Baseline = true,
				Hive = "HKLM",
				Process = ".*",
				Type = "set|create",
				Disabled = false,
				Index = "registry",
				MonitorSubnodes = true
			},
			ct)))
			.ShouldBePost(Path, "name=hklm&baseline=true&hive=HKLM&proc=.%2A&type=set%7Ccreate&disabled=false&index=registry&monitorSubnodes=true");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RegistryInputs.GetAsync("hklm", ct))).ShouldBeGet(Path + "/hklm");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RegistryInputs.UpdateAsync(
			"hklm", new RegistryInputUpdateRequest { Baseline = false, Hive = "HKCU", Process = "x", Type = "set" }, ct)))
			.ShouldBePost(Path + "/hklm", "baseline=false&hive=HKCU&proc=x&type=set");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.RegistryInputs.DeleteAsync("hklm", ct))).ShouldBeDelete(Path + "/hklm");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var input = (await InputsTestKit.MapEntryAsync((c, ct) => c.RegistryInputs.GetAsync("hklm", ct), RegistryJson)).Content!;

		input.Baseline.Should().BeFalse();
		input.Hive.Should().Be("HKLM");
		input.MonitorSubnodes.Should().BeTrue();
		input.Process.Should().Be("c:\\.*");
		input.Types.Should().Equal("set", "create", "delete", "rename");
	}

	[Fact]
	public Task ListAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.RegistryInputs.ListAsync(null, ct));
}
