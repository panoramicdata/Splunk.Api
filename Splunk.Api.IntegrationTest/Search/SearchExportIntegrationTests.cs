using Splunk.Api.Models.Search;

namespace Splunk.Api.IntegrationTest.Search;

[Collection(SplunkTestGroup.Name)]
public class SearchExportIntegrationTests(SplunkFixture fixture)
{
	private static readonly SearchExportRequest MakeResults = new() { Search = "| makeresults count=3 | eval x=1" };

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	private SplunkClient Client => fixture.Client;

	[Fact]
	public async Task ExportAsync_StreamsJsonLines()
	{
		await using var stream = await Client.SearchExport.ExportAsync(MakeResults, Ct);

		var records = await SearchExportReader.ReadAsync(stream, Ct).ToListAsync(Ct);

		records.Where(r => r.Result is not null).Should().HaveCount(3);
		records[^1].LastRow.Should().BeTrue();
		records[0].Result!["x"].Should().Be("1");
		records[0].Result!.Time.Should().NotBeNull();
	}

	[Fact]
	public async Task ExportCsvAsync_StreamsCsv()
	{
		using var reader = new StreamReader(await Client.SearchExport.ExportCsvAsync(MakeResults, Ct));

		var lines = (await reader.ReadToEndAsync(Ct)).Split('\n', StringSplitOptions.RemoveEmptyEntries);

		lines[0].Should().Be("\"_time\",x");
		lines.Should().HaveCount(4);
	}

	[Fact]
	public async Task ExportRawAsync_StreamsEventText()
	{
		using var reader = new StreamReader(await Client.SearchExport.ExportRawAsync(new SearchExportRequest { Search = "search index=_internal | head 2", EarliestTime = "-24h" }, Ct));

		var text = await reader.ReadToEndAsync(Ct);

		text.Split('\n', StringSplitOptions.RemoveEmptyEntries).Should().HaveCountGreaterThanOrEqualTo(2);
	}
}
