using Splunk.Api.Test.Support;
using System.Net;
using System.Text;

namespace Splunk.Api.Test.Core;

public class ErrorMapperTests
{
	private static async Task<SplunkApiException> MapAsync(HttpStatusCode status, HttpContent content, string? reason = null)
	{
		using var response = new HttpResponseMessage(status) { Content = content, ReasonPhrase = reason };
		var exception = await SplunkErrorMapper.CreateAsync(response);
		return exception.Should().BeOfType<SplunkApiException>().Subject;
	}

	private static Task<SplunkApiException> MapAsync(string body, HttpStatusCode status = HttpStatusCode.BadRequest)
		=> MapAsync(status, new StringContent(body));

	[Fact]
	public async Task Success_IsNotAnError()
	{
		using var response = new HttpResponseMessage(HttpStatusCode.NoContent);

		(await SplunkErrorMapper.CreateAsync(response)).Should().BeNull();
	}

	[Fact]
	public async Task Json_TheFirstErrorIsTheMessage_AndAllAreKept()
	{
		var exception = await MapAsync("""
			{"messages":[{"type":"WARN","text":"careful"},{"type":"error","text":""},{"type":"ERROR","text":"In handler 'savedsearch': bad"},{"text":1},7]}
			""", HttpStatusCode.Conflict);

		exception.StatusCode.Should().Be(HttpStatusCode.Conflict);
		exception.Message.Should().Be("In handler 'savedsearch': bad");
		exception.Messages.Select(m => m.ToString()).Should().Equal("WARN: careful", "error: ", "ERROR: In handler 'savedsearch': bad", ": ");
	}

	[Fact]
	public async Task Json_WithoutAnError_UsesTheFirstMessage()
		=> (await MapAsync("""  {"messages":[{"type":"WARN","text":"first"},{"type":"INFO","text":"second"}]}""")).Message.Should().Be("first");

	[Fact]
	public async Task Xml_MessagesAreRead_WithOrWithoutANamespace()
	{
		// An ordinary string, not a raw one: Lizard misreads the quotes of a raw literal that holds a URL.
		var exception = await MapAsync(
			"<?xml version=\"1.0\"?>\n<response xmlns:s=\"http://dev.splunk.com/ns/rest\"><messages><msg type=\"INFO\">note</msg>"
				+ "<s:msg type=\"ERROR\">\n  Unknown search command 'x'.\n</s:msg><msg>untyped</msg></messages></response>",
			HttpStatusCode.BadRequest);

		exception.Message.Should().Be("Unknown search command 'x'.");
		exception.Messages.Select(m => m.ToString()).Should().Equal("INFO: note", "ERROR: Unknown search command 'x'.", ": untyped");
	}

	[Theory]
	[InlineData("")]
	[InlineData("   ")]
	[InlineData("Internal error")]
	[InlineData("[1,2]")]
	[InlineData("{not json")]
	[InlineData("{}")]
	[InlineData("""{"messages":{}}""")]
	[InlineData("""{"messages":[]}""")]
	[InlineData("""{"messages":[{"type":"ERROR"}]}""")]
	[InlineData("<html><body>Bad gateway<br></body></html>")]
	[InlineData("<response/>")]
	[InlineData("<?xml version=\"1.0\"?><!DOCTYPE r [<!ENTITY x SYSTEM \"file:///etc/passwd\">]><r><msg>&x;</msg></r>")]
	public async Task NoReadableMessage_FallsBackToTheStatus(string body)
	{
		var exception = await MapAsync(body, HttpStatusCode.BadGateway);

		exception.Message.Should().Be("HTTP 502 (Bad Gateway)");
		exception.Messages.Should().NotContain(m => m.Text.Length > 0);
	}

	[Theory]
	[InlineData(null, "HTTP 599 (599)")]
	[InlineData("Custom", "HTTP 599 (Custom)")]
	public async Task Fallback_NamesTheReasonPhraseOrStatus(string? reason, string expected)
		=> (await MapAsync((HttpStatusCode)599, new StringContent(""), reason)).Message.Should().Be(expected);

	[Fact]
	public async Task UnsupportedCharset_FallsBackToTheStatus()
	{
		var content = new ByteArrayContent(Encoding.UTF8.GetBytes("""{"messages":[{"type":"ERROR","text":"x"}]}"""));
		content.Headers.TryAddWithoutValidation("Content-Type", "application/json; charset=no-such-charset");

		(await MapAsync(HttpStatusCode.InternalServerError, content)).Message.Should().Be("HTTP 500 (Internal Server Error)");
	}

	private sealed class FailingContent(Exception failure) : HttpContent
	{
		protected override Task SerializeToStreamAsync(Stream stream, System.Net.TransportContext? context)
		{
			ArgumentNullException.ThrowIfNull(stream);
			throw failure;
		}

		protected override bool TryComputeLength(out long length)
		{
			length = 0;
			return false;
		}
	}

	public static TheoryData<Exception> ReadFailures => new()
	{
		new HttpRequestException("reset"),
		new IOException("broken pipe")
	};

	[Theory]
	[MemberData(nameof(ReadFailures))]
	public async Task BodyLostWhileReading_FallsBackToTheStatus(Exception failure)
		=> (await MapAsync(HttpStatusCode.ServiceUnavailable, new FailingContent(failure))).Messages.Should().BeEmpty();

	[Fact]
	public async Task OtherReadFailures_Propagate()
	{
		using var response = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new FailingContent(new NotSupportedException("bug")) };

		var act = () => SplunkErrorMapper.CreateAsync(response);

		await act.Should().ThrowAsync<NotSupportedException>();
	}

	[Fact]
	public Task Client_RaisesTheMappedException()
		=> TestClient.ShouldFailAsync(
			(client, ct) => client.ServerInfo.GetAsync(ct),
			HttpStatusCode.NotFound,
			"""{"messages":[{"type":"ERROR","text":"Not found"}]}""",
			"Not found");

	[Fact]
	public async Task Client_FailedSessionLogin_RaisesTheLoginsError()
	{
		using var client = TestClient.Create(
			TestClient.Stub("""{"messages":[{"type":"WARN","text":"Login failed"}]}""", HttpStatusCode.Unauthorized),
			TestClient.UseSession);

		// The login's own error, not one that could echo the credentials.
		await TestClient.ShouldFailWithAsync(() => client.ServerInfo.GetAsync(TestContext.Current.CancellationToken), HttpStatusCode.Unauthorized, "Login failed");
	}
}
