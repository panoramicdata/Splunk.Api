using Splunk.Api.Models.Knowledge;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class MetricSchemasTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET data/transforms/metric-schema); the whitelist key added.
	private static readonly string SchemaJson = KnowledgeTestKit.Feed("data/transforms/metric-schema", "metric-schema:introspection_disk_objects", """
		{
			"METRIC-SCHEMA-BLACKLIST-DIMS": "datetime, *data.top_*, *data.args",
			"METRIC-SCHEMA-MEASURES": "_NUMS_EXCEPT_ *data.top_*",
			"METRIC-SCHEMA-WHITELIST-DIMS": "host",
			"METRIC-SCHEMA-MEASURES-queue": "current_size",
			"disabled": false,
			"eai:acl": null
		}
		""");

	[Fact]
	public async Task ListAsync_SendsGetToMetricSchema()
		=> (await KnowledgeTestKit.SendAsync(c => c.MetricSchemas.ListAsync(null, TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Get, "/services/data/transforms/metric-schema");

	[Fact]
	public async Task CreateAsync_PostsThePluralFieldNames()
		=> (await KnowledgeTestKit.SendAsync(c => c.MetricSchemas.CreateAsync(
				new MetricSchemaCreateRequest
				{
					Name = "splunk_metrics",
					FieldNames = "max_size_kb,current_size",
					BlacklistDimensions = "location,corp",
					WhitelistDimensions = "host",
					MetricNamePrefix = "queue"
				},
				TestContext.Current.CancellationToken)))
			.ShouldBe(
				HttpMethod.Post,
				"/services/data/transforms/metric-schema",
				body: "name=splunk_metrics&field_names=max_size_kb%2Ccurrent_size&blacklist_dimensions=location%2Ccorp&whitelist_dimensions=host&metric_name_prefix=queue");

	[Fact]
	public async Task DeleteAsync_SendsDeleteToTheSchema()
		=> (await KnowledgeTestKit.SendAsync(c => c.MetricSchemas.DeleteAsync("metric-schema:splunk_metrics", TestContext.Current.CancellationToken)))
			.ShouldBe(HttpMethod.Delete, "/services/data/transforms/metric-schema/metric-schema%3Asplunk_metrics");

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var entry = await KnowledgeTestKit.SingleEntryAsync(c => c.MetricSchemas.ListAsync(null, TestContext.Current.CancellationToken), SchemaJson);

		entry.ShouldBeTheCapturedEntry("metric-schema:introspection_disk_objects");
		entry.Content!.Measures.Should().Be("_NUMS_EXCEPT_ *data.top_*");
		entry.Content.BlacklistDimensions.Should().Be("datetime, *data.top_*, *data.args");
		entry.Content.WhitelistDimensions.Should().Be("host");
		entry.Content.Disabled.Should().BeFalse();
		entry.Content.AdditionalProperties["METRIC-SCHEMA-MEASURES-queue"].GetString().Should().Be("current_size");
	}

	[Fact]
	public Task DeleteAsync_NotFound_RaisesSplunkApiException()
		=> KnowledgeTestKit.ShouldRaiseNotFoundAsync(c => c.MetricSchemas.DeleteAsync("missing", TestContext.Current.CancellationToken));
}
