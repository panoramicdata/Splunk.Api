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
	public static StubHandler Stub(string json, System.Net.HttpStatusCode status = System.Net.HttpStatusCode.OK)
	{
		var stub = new StubHandler();
		stub.Enqueue(status, json);
		return stub;
	}
}
