using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class MetricsProcessorTests
{
	[Fact]
	public async Task ReloadAsync_PostsToReload()
		=> (await RequestAssert.SendAsync((c, ct) => c.MetricsProcessor.ReloadAsync(ct)))
			.ShouldBe(HttpMethod.Post, "/services/admin/metrics-reload/_reload");

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> RequestAssert.ShouldRaiseSplunkErrorAsync((c, ct) => c.MetricsProcessor.ReloadAsync(ct), System.Net.HttpStatusCode.Forbidden);
}
