using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class PipelineSetsTests
{
	// Captured from Splunk 10.6.0.5.
	private static readonly string PipelineJson = InputsTestKit.Feed("ingest_pipe_0", """
		{ "busiest_thread_name": "indexerPipe", "dutycycle_ratio": "0.00525064162256026", "eai:acl": null, "requests_last_period": "12", "share": "1" }
		""");

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.PipelineSets.ListAsync(ct))).ShouldBeGet("/services/server/pipelinesets");

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var entry = await InputsTestKit.MapEntryAsync((c, ct) => c.PipelineSets.ListAsync(ct), PipelineJson);

		entry.Name.Should().Be("ingest_pipe_0");
		entry.Content!.BusiestThreadName.Should().Be("indexerPipe");
		entry.Content.DutyCycleRatio.Should().BeApproximately(0.00525064162256026, 1e-12);
		entry.Content.RequestsLastPeriod.Should().Be(12);
		entry.Content.Share.Should().Be(1);
	}

	[Fact]
	public Task ListAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.PipelineSets.ListAsync(ct));
}
