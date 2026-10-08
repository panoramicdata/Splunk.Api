using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Applications;

/// <summary>Creates or installs an app (<c>POST apps/local</c>).</summary>
public sealed class AppCreateRequest : AppSettings
{
	/// <summary>
	/// The app name; or, when <see cref="Filename"/> is <see langword="true"/>, the path or URL of the package to install.
	/// </summary>
	[JsonPropertyName("name")]
	public required string Name { get; init; }

	/// <summary>The template to create the app from: <c>barebones</c> (Splunk's default), <c>sample_app</c>, or a custom one.</summary>
	[JsonPropertyName("template")]
	public string? Template { get; init; }

	/// <summary>Whether <see cref="Name"/> is the path or URL of a package to install rather than an app name.</summary>
	[JsonPropertyName("filename")]
	public bool? Filename { get; init; }

	/// <summary>The app name to install a package as, overriding the package's own (<c>explicit_appname</c>).</summary>
	[JsonPropertyName("explicit_appname")]
	public string? ExplicitAppName { get; init; }

	/// <summary>Whether installing from <see cref="Name"/> may update an existing app.</summary>
	[JsonPropertyName("update")]
	public bool? Update { get; init; }

	/// <summary>A Splunkbase session token, for installing or updating from Splunkbase. A secret.</summary>
	[JsonPropertyName("auth")]
	public string? Auth { get; init; }

	/// <summary>A Splunkbase login session token, an alternative to <see cref="Auth"/>. A secret.</summary>
	[JsonPropertyName("session")]
	public string? Session { get; init; }
}
