using Splunk.Api.Models.Introspection;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class DataSummariesTests
{
	[Fact]
	public async Task ListAsync_SendsTheSummaryKinds()
		=> await Calls.AssertAsync(
			c => c.DataSummaries.ListAsync(new SummaryListOptions { ReportAcceleration = true, DataModelAcceleration = false }, Calls.Token),
			HttpMethod.Get, "/services/data/summaries", "?report_acceleration=true&data_model_acceleration=false&output_mode=json", null);

	[Fact]
	public async Task GetAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.DataSummaries.GetAsync("dm_search_web", Calls.Token), HttpMethod.Get, "/services/data/summaries/dm_search_web", Calls.JsonQuery, null);

	[Fact]
	public async Task GetAsync_KeepsEveryProperty()
	{
		var feed = await Calls.MapAsync(c => c.DataSummaries.GetAsync("dm_search_web", Calls.Token), Feed.Of("dm_search_web", """{"eai:acl":null,"size":"12"}"""));

		feed.Entries.Should().ContainSingle().Which.Content!.AdditionalProperties["size"].GetString().Should().Be("12");
	}

	[Fact]
	public async Task GetAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.DataSummaries.GetAsync("nope", Calls.Token), HttpStatusCode.NotFound);
}
