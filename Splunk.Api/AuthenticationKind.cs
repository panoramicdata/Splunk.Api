namespace Splunk.Api;

/// <summary>How the client authenticates, derived from <see cref="SplunkClientOptions"/>.</summary>
internal enum AuthenticationKind
{
	/// <summary>A Splunk authentication token sent as a bearer token.</summary>
	Token,

	/// <summary>A session key obtained from <c>services/auth/login</c>, sent as <c>Authorization: Splunk</c>.</summary>
	Session,

	/// <summary>HTTP basic credentials on every request.</summary>
	Basic
}
