using System.Net;

namespace Splunk.Api.Test.Support;

internal static class TestClient
{
	public const string BaseUrl = "https://splunk.test:8089/";

	public static SplunkClient Create(StubHandler stub, Action<SplunkClientOptions>? tweak = null)
	{
		var options = new SplunkClientOptions
		{
			BaseUrl = BaseUrl,
			Token = "fake-token",
			MaxRetries = 0
		};
		tweak?.Invoke(options);
		return new SplunkClient(options, stub);
	}

	/// <summary>Switches options to session authentication (log in as <c>admin</c>/<c>secret</c>).</summary>
	public static void UseSession(SplunkClientOptions options)
	{
		options.Token = null;
		options.Username = "admin";
		options.Password = "secret";
	}

	/// <summary>A successful <c>services/auth/login</c> response carrying <paramref name="sessionKey"/>.</summary>
	public static string LoginJson(string sessionKey) => $$"""{"sessionKey":"{{sessionKey}}"}""";

	/// <summary>A stub that answers one request with <paramref name="json"/>.</summary>
	public static StubHandler Stub(string json, HttpStatusCode status = HttpStatusCode.OK)
	{
		var stub = new StubHandler();
		stub.Enqueue(status, json);
		return stub;
	}

	/// <summary>
	/// Sends <paramref name="call"/> through a client whose transport answers <paramref name="response"/>, and returns the
	/// one request it made. Every test kit's capture helper delegates here.
	/// </summary>
	public static async Task<RecordedCall> CaptureAsync(Func<SplunkClient, CancellationToken, Task> call, string response)
	{
		var stub = Stub(response);
		using var client = Create(stub);
		await call(client, TestContext.Current.CancellationToken);
		return stub.Calls.Should().ContainSingle().Subject;
	}

	/// <summary>Sends <paramref name="call"/> through a client whose transport answers <paramref name="response"/>, and returns what it read.</summary>
	public static async Task<T> ReadAsync<T>(Func<SplunkClient, CancellationToken, Task<T>> call, string response)
	{
		using var client = Create(Stub(response));
		return await call(client, TestContext.Current.CancellationToken);
	}

	/// <summary>Asserts that <paramref name="call"/> raises <see cref="SplunkApiException"/> when Splunk answers <paramref name="status"/> and <paramref name="response"/>.</summary>
	public static async Task ShouldFailAsync(Func<SplunkClient, CancellationToken, Task> call, HttpStatusCode status, string response, string message)
	{
		using var client = Create(Stub(response, status));
		await ShouldFailWithAsync(() => call(client, TestContext.Current.CancellationToken), status, message);
	}

	/// <summary>Asserts that <paramref name="act"/> raises <see cref="SplunkApiException"/> with the status and message.</summary>
	public static async Task ShouldFailWithAsync(Func<Task> act, HttpStatusCode status, string message)
	{
		var thrown = await act.Should().ThrowAsync<SplunkApiException>();
		thrown.Which.StatusCode.Should().Be(status);
		thrown.Which.Message.Should().Be(message);
	}
}
