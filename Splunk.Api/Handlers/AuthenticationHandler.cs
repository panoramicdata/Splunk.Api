using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Splunk.Api.Handlers;

/// <summary>
/// Authenticates each request: a bearer token, HTTP basic credentials, or a session key obtained from
/// <c>services/auth/login</c>. With a session, a 401 response invalidates the key, and the request is sent once more with
/// a fresh one when its body can be replayed. A failed login's response is returned in place of the request's, so it
/// surfaces as <see cref="SplunkApiException"/>. The options are copied at construction.
/// </summary>
internal sealed class AuthenticationHandler : DelegatingHandler
{
	internal const string LoginPath = "services/auth/login";

	private readonly AuthenticationKind _kind;
	private readonly string? _token;
	private readonly string? _username;
	private readonly string? _password;
	private readonly Uri _baseUri;
	private readonly Uri _loginUri;
	private readonly SemaphoreSlim _loginLock = new(1, 1);

	public AuthenticationHandler(SplunkClientOptions options, Uri baseUri)
	{
		_kind = options.AuthenticationKind;
		_token = options.Token;
		_username = options.Username;
		_password = options.Password;
		_baseUri = baseUri;
		_loginUri = new Uri(baseUri, LoginPath + "?" + OutputModeHandler.Parameter + "=json");
	}

	/// <summary>The current session key, if logged in.</summary>
	internal string? SessionKey { get; private set; }

	/// <inheritdoc />
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (_kind != AuthenticationKind.Session)
		{
			request.Headers.Authorization = _kind == AuthenticationKind.Token
				? new AuthenticationHeaderValue("Bearer", _token)
				: new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}")));
			return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
		}

		if (IsLoginRequest(request))
		{
			// An explicit call to the login endpoint carries its own credentials.
			return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
		}

		var session = await GetSessionAsync(null, cancellationToken).ConfigureAwait(false);
		if (session.Failure is { } loginFailure)
		{
			return InPlaceOf(loginFailure, request);
		}

		var response = await SendWithSessionAsync(request, session.Key!, cancellationToken).ConfigureAwait(false);
		if (response.StatusCode != HttpStatusCode.Unauthorized || !RetryHandler.IsReplayable(request.Content))
		{
			return response;
		}

		// The session expired or was revoked: log in again once, then resend.
		response.Dispose();
		session = await GetSessionAsync(session.Key, cancellationToken).ConfigureAwait(false);
		return session.Failure is { } reloginFailure
			? InPlaceOf(reloginFailure, request)
			: await SendWithSessionAsync(request, session.Key!, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Attributes a failed login's response to the caller's request. The transport set its request to the login, whose
	/// (disposed, but still referenced) form content holds the password.
	/// </summary>
	private static HttpResponseMessage InPlaceOf(HttpResponseMessage loginFailure, HttpRequestMessage request)
	{
		loginFailure.RequestMessage = request;
		return loginFailure;
	}

	private bool IsLoginRequest(HttpRequestMessage request)
		=> ServicePath.Relative(_baseUri, request.RequestUri!) is { } relative
			&& ServicePath.Endpoint(relative) is "auth/login" or "auth/login/";

	private Task<HttpResponseMessage> SendWithSessionAsync(HttpRequestMessage request, string key, CancellationToken cancellationToken)
	{
		request.Headers.Authorization = new AuthenticationHeaderValue("Splunk", key);
		return base.SendAsync(request, cancellationToken);
	}

	/// <summary>
	/// Returns the session key, logging in when there is none or when the current key is <paramref name="staleKey"/>
	/// (concurrent requests that all saw the same 401 log in only once).
	/// </summary>
	private async Task<LoginResult> GetSessionAsync(string? staleKey, CancellationToken cancellationToken)
	{
		var current = SessionKey;
		if (current is not null && current != staleKey)
		{
			return new(current, null);
		}

		await _loginLock.WaitAsync(cancellationToken).ConfigureAwait(false);
		try
		{
			current = SessionKey;
			if (current is not null && current != staleKey)
			{
				return new(current, null);
			}

			var result = await LoginAsync(cancellationToken).ConfigureAwait(false);
			SessionKey = result.Key;
			return result;
		}
		finally
		{
			_loginLock.Release();
		}
	}

	private async Task<LoginResult> LoginAsync(CancellationToken cancellationToken)
	{
		using var login = new HttpRequestMessage(HttpMethod.Post, _loginUri)
		{
			Content = new FormUrlEncodedContent(
			[
				new("username", _username!),
				new("password", _password!)
			])
		};
		var response = await base.SendAsync(login, cancellationToken).ConfigureAwait(false);
		if (!response.IsSuccessStatusCode)
		{
			return new(null, response);
		}

		using (response)
		{
			var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			return new(ReadSessionKey(body), null);
		}
	}

	internal static string ReadSessionKey(string body)
	{
		try
		{
			using var document = JsonDocument.Parse(body);
			if (document.RootElement.ValueKind == JsonValueKind.Object
				&& document.RootElement.TryGetProperty("sessionKey", out var key)
				&& key.ValueKind == JsonValueKind.String
				&& key.GetString()!.Length > 0)
			{
				return key.GetString()!;
			}
		}
		catch (JsonException)
		{
			// Fall through: the body is not the expected JSON.
		}

		throw new InvalidOperationException("Splunk's login response did not contain a session key.");
	}

	/// <inheritdoc />
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_loginLock.Dispose();
		}

		base.Dispose(disposing);
	}

	/// <summary>A session key, or the failed login response to return in place of the request's.</summary>
	private sealed record LoginResult(string? Key, HttpResponseMessage? Failure);
}
