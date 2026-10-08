using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class AllInputsTests
{
	private const string Path = "/services/data/inputs/all";

	// Captured from Splunk 10.6.0.5; host names replaced.
	private static readonly string InputJson = InputsTestKit.Feed("9997", """
		{
			"_rcvbuf": 1572864, "disabled": false, "eai:acl": null, "eai:location": "/data/inputs/tcp/cooked", "eai:type": "cooked",
			"group": "listenerports", "host": "$decideOnStartup", "host_resolved": "splunk01", "index": "default"
		}
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.AllInputs.ListAsync(new DataInputListOptions { Common = true, Count = 2 }, ct)))
			.ShouldBe(HttpMethod.Get, Path, "?common=true&count=2&output_mode=json", null);

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.AllInputs.GetAsync("9997", new DataInputOptions { Common = false }, ct)))
			.ShouldBe(HttpMethod.Get, Path + "/9997", "?common=false&output_mode=json", null);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var input = (await InputsTestKit.MapEntryAsync((c, ct) => c.AllInputs.GetAsync("9997", null, ct), InputJson)).Content!;

		input.Kind.Should().Be("cooked");
		input.Location.Should().Be("/data/inputs/tcp/cooked");
		input.HostResolved.Should().Be("splunk01");
		input.AdditionalProperties["group"].GetString().Should().Be("listenerports");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.AllInputs.GetAsync("nope", null, ct));
}
