using Splunk.Api.Models.Introspection;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class IndexesExtendedTests
{
	[Fact]
	public async Task ListAsync_SendsTheDataType()
		=> await Calls.AssertAsync(
			c => c.IndexesExtended.ListAsync(new IndexListOptions { DataType = IndexDataType.Metric }, Calls.Token),
			HttpMethod.Get, "/services/data/indexes-extended", "?datatype=metric&output_mode=json", null);

	[Fact]
	public async Task GetAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.IndexesExtended.GetAsync("main", Calls.Token), HttpMethod.Get, "/services/data/indexes-extended/main", Calls.JsonQuery, null);

	[Fact]
	public async Task GetAsync_MapsTheIndexAndItsSize()
	{
		// Captured from Splunk 10.6.0.5: the extended view adds name and total_size to the index properties.
		var content = IndexesTests.MainContent.TrimEnd().TrimEnd('}') + """, "name": "main", "total_size": "1.250" }""";

		var feed = await Calls.MapAsync(c => c.IndexesExtended.GetAsync("main", Calls.Token), Feed.Of("main", content));

		var index = feed.Entries.Should().ContainSingle().Subject.Content!;
		IndexesTests.AssertMain(index);
		index.Name.Should().Be("main");
		index.TotalSizeMB.Should().Be(1.25);
	}

	[Fact]
	public async Task GetAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.IndexesExtended.GetAsync("nope", Calls.Token), HttpStatusCode.NotFound);
}
