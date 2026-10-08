using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class PerfmonInputsTests
{
	private const string Path = "/services/data/inputs/win-perfmon";

	// Shaped as the reference's example (a Linux test instance has no Performance Monitor inputs).
	private static readonly string PerfmonJson = InputsTestKit.Feed("memory", """
		{
			"counters": "Available Bytes", "disabled": "1", "eai:acl": null, "index": "default", "instances": [], "interval": "10",
			"nonmetric_counters": null, "object": "Memory"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.PerfmonInputs.ListAsync(null, ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.PerfmonInputs.CreateAsync(
			new PerfmonInputCreateRequest
			{
				Name = "memory",
				Counters = ["Available Bytes", "Pages/sec"],
				Host = "web01",
				Index = "perfmon",
				Instances = ["*"],
				Interval = 10,
				PerformanceObject = "Memory",
				Source = "perfmon",
				Sourcetype = "Perfmon:Memory"
			},
			ct)))
			.ShouldBePost(Path, "name=memory&counters=Available+Bytes&counters=Pages%2Fsec&host=web01&index=perfmon&instances=%2A&interval=10&object=Memory&source=perfmon&sourcetype=Perfmon%3AMemory");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.PerfmonInputs.GetAsync("memory", ct))).ShouldBeGet(Path + "/memory");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.PerfmonInputs.UpdateAsync("memory", new PerfmonInputUpdateRequest { Interval = 30 }, ct)))
			.ShouldBePost(Path + "/memory", "interval=30");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.PerfmonInputs.DeleteAsync("memory", ct))).ShouldBeDelete(Path + "/memory");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var input = (await InputsTestKit.MapEntryAsync((c, ct) => c.PerfmonInputs.GetAsync("memory", ct), PerfmonJson)).Content!;

		input.Counters.Should().Equal("Available Bytes");
		input.Instances.Should().BeEmpty();
		input.Interval.Should().Be(10);
		input.NonMetricCounters.Should().BeEmpty();
		input.PerformanceObject.Should().Be("Memory");
	}

	[Fact]
	public Task ListAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.PerfmonInputs.ListAsync(null, ct));
}
