namespace Splunk.Api;

/// <summary>
/// Configuration for <see cref="SplunkClient"/>. The values are read once when the client is constructed; changing this
/// object afterwards does not affect an existing client.
/// </summary>
/// <remarks>
/// Supply exactly one way to authenticate: a <see cref="Token"/> (a Splunk authentication token, sent as a bearer token,
/// which Splunk recommends), or a <see cref="Username"/> and <see cref="Password"/>. With a username and password the client
/// logs in through <c>services/auth/login</c>, sends the session key it receives, and logs in again once if Splunk answers
/// 401 because the session expired; set <see cref="UseBasicAuthentication"/> to send HTTP basic credentials on every
/// request instead.
/// </remarks>
public class SplunkClientOptions : SplunkConnectionOptions
{
	/// <summary>
	/// Absolute URL of the Splunk management port, e.g. <c>https://splunk.example.com:8089</c>. A path prefix (for a
	/// reverse proxy) is kept: every endpoint is appended to it. It must not contain credentials (<c>user:password@</c>).
	/// </summary>
	public string BaseUrl { get; set; } = string.Empty;

	/// <summary>A Splunk authentication token, sent as <c>Authorization: Bearer</c>. Mutually exclusive with <see cref="Username"/>.</summary>
	public string? Token { get; set; }

	/// <summary>The Splunk user name for session or basic authentication. Requires <see cref="Password"/>.</summary>
	public string? Username { get; set; }

	/// <summary>The password for <see cref="Username"/>.</summary>
	public string? Password { get; set; }

	/// <summary>
	/// Send HTTP basic credentials on every request instead of logging in for a session key. Requires
	/// <see cref="Username"/> and <see cref="Password"/>. Defaults to <see langword="false"/>.
	/// </summary>
	public bool UseBasicAuthentication { get; set; }

	/// <summary>
	/// The default namespace (user and app context) for namespaced endpoints. When set, requests to <c>services/...</c>
	/// are sent to <c>servicesNS/{owner}/{app}/...</c>. Leave <see langword="null"/> to use the global
	/// <c>services/...</c> context. A different namespace can be used per call through <see cref="SplunkClient.InNamespace(SplunkNamespace)"/>.
	/// </summary>
	public SplunkNamespace? Namespace { get; set; }

	/// <summary>
	/// When <see langword="true"/>, the client refuses (with <see cref="SplunkReadOnlyException"/>, before anything is
	/// sent) every request that could change Splunk: any DELETE, and any POST except the read-only operations on an
	/// allow-list (logging in, creating, controlling and exporting search jobs, and parsing or previewing searches).
	/// </summary>
	/// <remarks>
	/// This guards the REST surface, not the SPL inside a search: a search that runs <c>| delete</c>, <c>| outputlookup</c>
	/// or <c>| collect</c> still runs. Use a Splunk role without those capabilities for a truly read-only identity.
	/// </remarks>
	public bool ReadOnly { get; set; }

	internal AuthenticationKind AuthenticationKind
		=> !string.IsNullOrWhiteSpace(Token)
			? AuthenticationKind.Token
			: UseBasicAuthentication ? AuthenticationKind.Basic : AuthenticationKind.Session;

	internal void Validate()
	{
		if (!TryParseBaseUrl(BaseUrl, out var baseUri))
		{
			throw new ArgumentException("BaseUrl must be an absolute http or https URL.", nameof(BaseUrl));
		}

		// HttpClient never sends user info, and it would appear in logs and exception messages that show the URL.
		if (baseUri.UserInfo.Length > 0)
		{
			throw new ArgumentException("BaseUrl must not contain credentials: set Token, or Username and Password.", nameof(BaseUrl));
		}

		// Endpoint paths are appended to BaseUrl, which a query string or fragment would swallow.
		if (baseUri.Query.Length > 0 || baseUri.Fragment.Length > 0)
		{
			throw new ArgumentException("BaseUrl must not have a query string or fragment.", nameof(BaseUrl));
		}

		ValidateCredentials();
		_ = Namespace?.PathPrefix;
		ValidateConnection();
	}

	private void ValidateCredentials()
	{
		var hasToken = !string.IsNullOrWhiteSpace(Token);
		var hasUser = !string.IsNullOrWhiteSpace(Username);
		if (hasToken && (hasUser || UseBasicAuthentication))
		{
			throw new ArgumentException("Set either Token, or Username and Password, not both.", nameof(Token));
		}

		if (!hasToken && (!hasUser || string.IsNullOrEmpty(Password)))
		{
			throw new ArgumentException("Set Token, or both Username and Password.", nameof(Username));
		}
	}

	/// <inheritdoc />
	public override string ToString()
		=> $"SplunkClientOptions {{ BaseUrl = {MaskUserInfo(BaseUrl)}, Token = {Mask(Token)}, Username = {Username}, Password = {Mask(Password)}, Namespace = {Namespace}, ReadOnly = {ReadOnly} }}";

	private static string Mask(string? secret) => string.IsNullOrEmpty(secret) ? "(none)" : "***";

	/// <summary>
	/// Masks any <c>user:password@</c> in a URL. Done on the text rather than a parsed <see cref="Uri"/>, since this must
	/// also redact a URL that fails to parse (such as one whose password contains an unescaped <c>@</c>).
	/// </summary>
	private static string MaskUserInfo(string url)
	{
		var start = url.IndexOf("://", StringComparison.Ordinal);
		if (start < 0)
		{
			return url;
		}

		start += 3;
		var end = url.IndexOfAny(['/', '?', '#'], start);
		var authority = end < 0 ? url[start..] : url[start..end];
		var at = authority.LastIndexOf('@');
		return at < 0 ? url : $"{url[..start]}***{url[(start + at)..]}";
	}
}
