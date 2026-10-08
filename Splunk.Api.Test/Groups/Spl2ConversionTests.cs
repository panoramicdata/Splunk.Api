using Splunk.Api.Models;
using Splunk.Api.Models.Spl2;
using Splunk.Api.Test.Support;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class Spl2ConversionTests
{
	private const string RequestJson = """{"spl1":"| sdselect count from main","runtime":"splunkd"}""";

	private static readonly JsonBody<Spl2ConversionRequest> Request = new(new Spl2ConversionRequest { Spl = "| sdselect count from main" });

	private static CancellationToken Ct => TestContext.Current.CancellationToken;

	[Fact]
	public async Task ConvertAsync_SendsJson()
	{
		var stub = TestClient.Stub("""{"spl2":"FROM main SELECT count()","messages":"Unable to convert sdselect to a SEARCH command, using the FROM command."}""");
		using var client = TestClient.Create(stub);

		var result = await client.Spl2Conversion.ConvertAsync(Request, Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, "/services/orchestrator/v1/spl2/convert", "?output_mode=json", RequestJson);
		stub.Calls[0].ContentType.Should().Be("application/json");
		result.Spl2.Should().Be("FROM main SELECT count()");
		result.Messages.Should().StartWith("Unable to convert sdselect");
	}

	[Fact]
	public async Task ConvertV2Async_SendsJson()
	{
		var stub = TestClient.Stub("""{"spl2":"FROM main SELECT count()","message":{"type":"warn","code":"SPL2_CONVERT_WARN","message":"Converted with warnings."}}""");
		using var client = TestClient.Create(stub);

		var result = await client.Spl2Conversion.ConvertV2Async(new JsonBody<Spl2ConversionRequest>(new Spl2ConversionRequest { Spl = "| sdselect count from main", Runtime = "edge" }), Ct);

		SearchRequestAssert.Sent(stub, HttpMethod.Post, "/services/orchestrator/v2/spl2/convert", "?output_mode=json", """{"spl1":"| sdselect count from main","runtime":"edge"}""");
		result.Spl2.Should().Be("FROM main SELECT count()");
		result.Message!.Type.Should().Be("warn");
		result.Message.Code.Should().Be("SPL2_CONVERT_WARN");
		result.Message.Text.Should().Be("Converted with warnings.");
	}

	[Fact]
	public async Task ConvertV2Async_WithoutAWarning_HasNoMessage()
	{
		using var client = TestClient.Create(TestClient.Stub("""{"spl2":"FROM main | stats count() by host"}"""));

		var result = await client.Spl2Conversion.ConvertV2Async(Request, Ct);

		result.Message.Should().BeNull();
	}

	[Fact]
	public async Task ConvertAsync_LanguageServerDown_RaisesItsMessage()
	{
		const string Down = "Failed to open resource handle: uds:///opt/splunk/var/run/splunk/orchestrator/lsp-13711.sock (failed to dial unix after retries)";
		using var client = TestClient.Create(TestClient.Stub($$"""{"code":"500","message":"{{Down}}"}""", HttpStatusCode.InternalServerError));

		await SearchRequestAssert.FailsWith(() => client.Spl2Conversion.ConvertAsync(Request, Ct), HttpStatusCode.InternalServerError, Down);
	}
}
