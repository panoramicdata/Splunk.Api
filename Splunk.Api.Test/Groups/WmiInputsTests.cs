using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class WmiInputsTests
{
	private const string Path = "/services/data/inputs/win-wmi-collections";

	// Shaped as the reference's example (a Linux test instance has no WMI collections).
	private static readonly string WmiJson = InputsTestKit.Feed("cpu", """
		{
			"classes": "Win32_PerfFormattedData_PerfOS_Processor", "disabled": "1", "eai:acl": null,
			"fields": ["PercentProcessorTime", "PercentUserTime"], "index": "default", "instances": "_Total", "interval": "3",
			"lookup_host": "localhost", "server": "localhost",
			"wql": "SELECT PercentProcessorTime,PercentUserTime FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name=\"_Total\""
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.WmiInputs.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.WmiInputs.CreateAsync(
			new WmiInputCreateRequest
			{
				Name = "cpu",
				Classes = "Win32_PerfFormattedData_PerfOS_Processor",
				Interval = 3,
				LookupHost = "localhost",
				Disabled = false,
				Fields = ["PercentProcessorTime", "PercentUserTime"],
				Index = "wmi",
				Instances = ["_Total"],
				Server = "web01,web02"
			},
			ct)))
			.ShouldBePost(Path, "name=cpu&classes=Win32_PerfFormattedData_PerfOS_Processor&interval=3&lookup_host=localhost&disabled=false&fields=PercentProcessorTime&fields=PercentUserTime&index=wmi&instances=_Total&server=web01%2Cweb02");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.WmiInputs.GetAsync("cpu", ct))).ShouldBeGet(Path + "/cpu");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.WmiInputs.UpdateAsync(
			"cpu", new WmiInputUpdateRequest { Classes = "Win32_Process", Interval = 10, LookupHost = "localhost" }, ct)))
			.ShouldBePost(Path + "/cpu", "classes=Win32_Process&interval=10&lookup_host=localhost");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.WmiInputs.DeleteAsync("cpu", ct))).ShouldBeDelete(Path + "/cpu");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var input = (await InputsTestKit.MapEntryAsync((c, ct) => c.WmiInputs.GetAsync("cpu", ct), WmiJson)).Content!;

		input.Classes.Should().Be("Win32_PerfFormattedData_PerfOS_Processor");
		input.Fields.Should().Equal("PercentProcessorTime", "PercentUserTime");
		input.Instances.Should().Equal("_Total");
		input.Interval.Should().Be(3);
		input.LookupHost.Should().Be("localhost");
		input.Server.Should().Be("localhost");
		input.Wql.Should().StartWith("SELECT PercentProcessorTime");
	}

	[Fact]
	public Task ListAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.WmiInputs.ListAsync(null, ct));
}
