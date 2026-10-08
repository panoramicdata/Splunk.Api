using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class StatsdExtractionsTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (POST data/transforms/statsdextractions), names changed.
	private static readonly string ExtractionJson = KnowledgeTestKit.Feed("data/transforms/statsdextractions", "statsd-dims:statsd-ex", """
		{
			"REGEX": "[.](?<hostname>[^.]+)[.]",
			"REMOVE_DIMS_FROM_METRIC_NAME": true,
			"disabled": false,
			"eai:acl": null
		}
		""");

	[Fact]
	public async Task CreateAsync_PostsNameRegexAndFlag()
		=> (await KnowledgeTestKit.SendAsync(
				c => c.StatsdExtractions.CreateAsync(new StatsdExtractionCreateRequest { Name = "statsd-ex", Regex = "[.](?<hostname>[^.]+)[.]", RemoveDimensionsFromMetricName = true }, TestContext.Current.CancellationToken),
				ExtractionJson))
			.ShouldBe(
				HttpMethod.Post,
				"/services/data/transforms/statsdextractions",
				body: "name=statsd-ex&REGEX=%5B.%5D%28%3F%3Chostname%3E%5B%5E.%5D%2B%29%5B.%5D&REMOVE_DIMS_FROM_METRIC_NAME=true");

	[Fact]
	public async Task CreateAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(
			c => c.StatsdExtractions.CreateAsync(new StatsdExtractionCreateRequest { Name = "statsd-ex", Regex = "x" }, TestContext.Current.CancellationToken),
			ExtractionJson);

		entry.ShouldBeTheCapturedEntry("statsd-dims:statsd-ex");
		entry.Content!.Regex.Should().Be("[.](?<hostname>[^.]+)[.]");
		entry.Content.RemoveDimensionsFromMetricName.Should().BeTrue();
		entry.Content.Disabled.Should().BeFalse();
	}

	[Fact]
	public Task CreateAsync_Error_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.StatsdExtractions.CreateAsync(new StatsdExtractionCreateRequest { Name = "n", Regex = "x" }, TestContext.Current.CancellationToken));
}
