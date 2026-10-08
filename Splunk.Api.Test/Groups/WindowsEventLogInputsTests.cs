using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class WindowsEventLogInputsTests
{
	private const string Path = "/services/data/inputs/win-event-log-collections";

	// Shaped as the reference's example (a Linux test instance has no event log collections).
	private static readonly string EventLogJson = InputsTestKit.Feed("localhost", """
		{
			"disabled": "1", "eai:acl": null, "hosts": "localhost", "index": "default", "lookup_host": "localhost",
			"logs": ["Application", "Security", "System"]
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.WindowsEventLogInputs.ListAsync(new WindowsEventLogInputListOptions { LookupHost = "localhost" }, ct)))
			.ShouldBe(HttpMethod.Get, Path, "?lookup_host=localhost&output_mode=json", null);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.WindowsEventLogInputs.CreateAsync(
			new WindowsEventLogInputCreateRequest
			{
				Name = "localhost",
				LookupHost = "localhost",
				Hosts = "web01,web02",
				Index = "wineventlog",
				Logs = ["Application", "System"]
			},
			ct)))
			.ShouldBePost(Path, "name=localhost&lookup_host=localhost&hosts=web01%2Cweb02&index=wineventlog&logs=Application&logs=System");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.WindowsEventLogInputs.GetAsync("localhost", new WindowsEventLogInputOptions { LookupHost = "localhost" }, ct)))
			.ShouldBe(HttpMethod.Get, Path + "/localhost", "?lookup_host=localhost&output_mode=json", null);

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.WindowsEventLogInputs.UpdateAsync(
			"localhost", new WindowsEventLogInputUpdateRequest { LookupHost = "localhost" }, ct)))
			.ShouldBePost(Path + "/localhost", "lookup_host=localhost");

	[Fact]
	public async Task DeleteAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.WindowsEventLogInputs.DeleteAsync("localhost", ct))).ShouldBeDelete(Path + "/localhost");

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var input = (await InputsTestKit.MapEntryAsync((c, ct) => c.WindowsEventLogInputs.GetAsync("localhost", null, ct), EventLogJson)).Content!;

		input.Hosts.Should().Be("localhost");
		input.LookupHost.Should().Be("localhost");
		input.Logs.Should().Equal("Application", "Security", "System");
	}

	[Fact]
	public Task ListAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.WindowsEventLogInputs.ListAsync(null, ct));
}
