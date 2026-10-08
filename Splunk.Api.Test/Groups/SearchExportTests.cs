using Splunk.Api.Models.Search;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class SearchExportTests
{
	private const string ExportPath = "/services/search/v2/jobs/export";
	private const string ExportBody = "search=search+index%3D_internal+%7C+head+2&earliest_time=-1h";

	private static readonly SearchExportRequest Request = new() { Search = "search index=_internal | head 2", EarliestTime = "-1h" };

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ExportAsync_SendsPostAndStreamsJsonLines()
	{
		var stub = SearchRequestAssert.TextStub(SearchJson.Export, "application/json");
		using var client = TestClient.Create(stub);

		await using var stream = await client.SearchExport.ExportAsync(Request, Ct);
		var records = await SearchExportReader.ReadAsync(stream, Ct).ToListAsync(Ct);

		stub.ShouldHaveSent(HttpMethod.Post, ExportPath, "?output_mode=json", ExportBody);
		records.Should().HaveCount(4);
		records[0].Preview.Should().BeTrue();
		records[1].Offset.Should().Be(0);
		records[1].LastRow.Should().BeFalse();
		records[1].Result!["x"].Should().Be("1");
		records[1].Result!.Time.Should().Be(new DateTimeOffset(2026, 10, 8, 13, 48, 34, TimeSpan.Zero));
		records[2].LastRow.Should().BeTrue();
		records[2].Result!.GetValues("x").Should().Equal("1", "2");
		records[3].Result.Should().BeNull();
		records[3].Offset.Should().BeNull();
		records[3].Messages.Should().BeEmpty();
	}

	[Fact]
	public async Task ExportCsvAsync_SendsPostForCsv()
	{
		var stub = SearchRequestAssert.TextStub("\"_time\",x\n\"2026-10-08 13:48:57.000 GMT\",1\n", "text/csv");
		using var client = TestClient.Create(stub);

		using var reader = new StreamReader(await client.SearchExport.ExportCsvAsync(Request, Ct));

		stub.ShouldHaveSent(HttpMethod.Post, ExportPath, "?output_mode=csv", ExportBody);
		(await reader.ReadLineAsync(Ct)).Should().Be("\"_time\",x");
	}

	[Fact]
	public async Task ExportRawAsync_SendsPostForRawText()
	{
		var stub = SearchRequestAssert.TextStub("raw event one\nraw event two\n", "text/plain");
		using var client = TestClient.Create(stub);

		using var reader = new StreamReader(await client.SearchExport.ExportRawAsync(Request, Ct));

		stub.ShouldHaveSent(HttpMethod.Post, ExportPath, "?output_mode=raw", ExportBody);
		(await reader.ReadLineAsync(Ct)).Should().Be("raw event one");
	}

	[Fact]
	public async Task ExportAsync_Error_RaisesSplunkApiException()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"FATAL","text":"Unknown search command 'nosuchcommand'."}]}""", HttpStatusCode.BadRequest));

		var act = () => client.SearchExport.ExportAsync(new SearchExportRequest { Search = "| nosuchcommand" }, Ct);

		await act.ShouldFailWith(HttpStatusCode.BadRequest, "Unknown search command 'nosuchcommand'.");
	}

	[Fact]
	public async Task ReadAsync_NullLine_Throws()
	{
		using var stream = new MemoryStream("null\n"u8.ToArray());

		var act = async () => await SearchExportReader.ReadAsync(stream, Ct).ToListAsync(Ct);

		await act.Should().ThrowAsync<System.Text.Json.JsonException>();
	}

	[Fact]
	public async Task ReadAsync_RejectsANullStream()
	{
		var act = async () => await SearchExportReader.ReadAsync(null!, Ct).ToListAsync(Ct);

		await act.Should().ThrowAsync<ArgumentNullException>();
	}
}
