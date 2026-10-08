namespace Splunk.Api.Test.Support;

/// <summary>Assertions on a request recorded by <see cref="StubHandler"/>, shared by every endpoint group's tests.</summary>
internal static class RecordedCallAssertions
{
	/// <summary>The query string every request carries when the operation sends no parameters of its own.</summary>
	public const string JsonQuery = "?output_mode=json";

	/// <summary>Asserts the method, escaped path, query and body of a recorded request; every test kit's request assertion builds on it.</summary>
	public static void ShouldMatch(this RecordedCall call, HttpMethod method, string path, string query, string? body)
	{
		call.Method.Should().Be(method);
		call.Uri.AbsolutePath.Should().Be(path);
		call.Uri.Query.Should().Be(query);
		call.Body.Should().Be(body);
	}

	/// <summary>
	/// Asserts the method, escaped path, query and body of a recorded request. A body that is not deliberately JSON, plain
	/// text or binary (which the calling test asserts itself) must be sent as <c>application/x-www-form-urlencoded</c>.
	/// </summary>
	public static void ShouldBe(this RecordedCall call, HttpMethod method, string path, string query = JsonQuery, string? body = null)
	{
		call.ShouldMatch(method, path, query, body);
		if (body is not null && call.ContentType is not ("application/json" or "text/plain" or "application/octet-stream"))
		{
			call.ContentType.Should().Be("application/x-www-form-urlencoded");
		}
	}
}
