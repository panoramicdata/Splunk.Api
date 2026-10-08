using System.Net;

namespace Splunk.Api.Test.Support;

/// <summary>
/// One-line request pinning for endpoint tests: send one call through a stubbed client and assert the exact request, or
/// assert how an error response surfaces.
/// </summary>
internal static class RequestAssert
{
	/// <summary>An empty feed, the response of most write operations.</summary>
	public const string EmptyFeed = """{"links":{},"entry":[],"paging":{"total":0,"perPage":30,"offset":0},"messages":[]}""";

	/// <summary>The query every JSON request carries when the call adds none of its own.</summary>
	public const string JsonQuery = "?output_mode=json";

	/// <summary>Sends <paramref name="call"/> to a client whose transport answers <paramref name="response"/>, and returns the one request.</summary>
	public static async Task<RecordedCall> SendAsync(Func<SplunkClient, CancellationToken, Task> call, string response = EmptyFeed)
	{
		var stub = TestClient.Stub(response);
		using var client = TestClient.Create(stub);

		await call(client, TestContext.Current.CancellationToken);

		return stub.Calls.Should().ContainSingle().Subject;
	}

	/// <summary>Sends <paramref name="call"/> to a client whose transport answers <paramref name="response"/>, and returns the result.</summary>
	public static async Task<T> ReadAsync<T>(Func<SplunkClient, CancellationToken, Task<T>> call, string response)
	{
		using var client = TestClient.Create(TestClient.Stub(response));
		return await call(client, TestContext.Current.CancellationToken);
	}

	/// <summary>Asserts the request's method, path, query and form body (<see langword="null"/> for none).</summary>
	public static void ShouldBe(this RecordedCall call, HttpMethod method, string path, string? body = null, string query = JsonQuery)
	{
		call.Method.Should().Be(method);
		call.Uri.AbsolutePath.Should().Be(path);
		call.Uri.Query.Should().Be(query);
		call.Body.Should().Be(body);
		if (body is not null)
		{
			call.ContentType.Should().Be("application/x-www-form-urlencoded");
		}
	}

	/// <summary>Asserts that <paramref name="call"/> raises <see cref="SplunkApiException"/> carrying Splunk's error message.</summary>
	public static async Task ShouldRaiseSplunkErrorAsync(Func<SplunkClient, CancellationToken, Task> call, HttpStatusCode status = HttpStatusCode.NotFound)
	{
		const string Text = "Could not find object id=missing";
		using var client = TestClient.Create(TestClient.Stub($$"""{"messages":[{"type":"ERROR","text":"{{Text}}"}]}""", status));

		var act = () => call(client, TestContext.Current.CancellationToken);

		var thrown = await act.Should().ThrowAsync<SplunkApiException>();
		thrown.Which.StatusCode.Should().Be(status);
		thrown.Which.Message.Should().Be(Text);
	}
}
