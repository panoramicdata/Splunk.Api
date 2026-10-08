using System.Net;

namespace Splunk.Api.Test.Support.Platform;

/// <summary>Sends one call through a stubbed client and asserts what went over the wire.</summary>
internal static class Calls
{
	/// <summary>The query every JSON call carries when it sets nothing else.</summary>
	public const string JsonQuery = "?output_mode=json";

	/// <summary>A stub answering with an empty feed.</summary>
	public const string EmptyFeed = """{"entry":[],"messages":[]}""";

	/// <summary>Runs <paramref name="call"/> against a client answering <paramref name="json"/> and returns the one recorded request.</summary>
	public static async Task<RecordedCall> RecordAsync(Func<SplunkClient, Task> call, string json)
	{
		var stub = TestClient.Stub(json);
		using var client = TestClient.Create(stub);

		await call(client);

		return stub.Calls.Should().ContainSingle().Subject;
	}

	/// <summary>Runs <paramref name="call"/> and asserts the request's method, unescaped path, raw query and form body.</summary>
	public static async Task<RecordedCall> AssertAsync(
		Func<SplunkClient, Task> call,
		HttpMethod method,
		string path,
		string query,
		string? body)
	{
		var recorded = await RecordAsync(call, EmptyFeed);

		recorded.Method.Should().Be(method);
		recorded.Uri.AbsolutePath.Should().Be(path);
		recorded.Uri.Query.Should().Be(query);
		recorded.Body.Should().Be(body);
		return recorded;
	}

	/// <summary>Runs <paramref name="call"/> against a client that answers <paramref name="json"/> and returns the result.</summary>
	public static async Task<T> MapAsync<T>(Func<SplunkClient, Task<T>> call, string json)
	{
		using var client = TestClient.Create(TestClient.Stub(json));
		return await call(client);
	}

	/// <summary>Asserts that a Splunk error response raises <see cref="SplunkApiException"/> carrying its message.</summary>
	public static async Task AssertErrorAsync(Func<SplunkClient, Task> call, HttpStatusCode status)
	{
		using var client = TestClient.Create(TestClient.Stub("""{"messages":[{"type":"ERROR","text":"Splunk says no"}]}""", status));

		var act = () => call(client);

		var thrown = await act.Should().ThrowAsync<SplunkApiException>();
		thrown.Which.StatusCode.Should().Be(status);
		thrown.Which.Message.Should().Be("Splunk says no");
	}

	/// <summary>The test's cancellation token.</summary>
	public static CancellationToken Token => TestContext.Current.CancellationToken;
}
