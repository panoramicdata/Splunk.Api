using Splunk.Api.Serialization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>The properties of a user, shared by <see cref="User"/> and <see cref="CurrentContext"/>.</summary>
public abstract class UserProperties : SplunkContent
{
	/// <summary>Every capability the user has, through all of its roles.</summary>
	[JsonPropertyName("capabilities")]
	public IReadOnlyList<string> Capabilities { get; init; } = [];

	/// <summary>The roles assigned to the user.</summary>
	[JsonPropertyName("roles")]
	public IReadOnlyList<string> Roles { get; init; } = [];

	/// <summary>The user's full name.</summary>
	[JsonPropertyName("realname")]
	public string? RealName { get; init; }

	/// <summary>The user's email address.</summary>
	[JsonPropertyName("email")]
	public string? Email { get; init; }

	/// <summary>The app opened at login.</summary>
	[JsonPropertyName("defaultApp")]
	public string? DefaultApp { get; init; }

	/// <summary>Whether <see cref="DefaultApp"/> is set on the user, overriding the roles' default app.</summary>
	[JsonPropertyName("defaultAppIsUserOverride")]
	public bool? DefaultAppIsUserOverride { get; init; }

	/// <summary>The role <see cref="DefaultApp"/> comes from, or <c>system</c>.</summary>
	[JsonPropertyName("defaultAppSourceRole")]
	public string? DefaultAppSourceRole { get; init; }

	/// <summary>The authentication system: <c>Splunk</c>, <c>LDAP</c>, <c>Scripted</c>, <c>SAML</c> or <c>System</c>.</summary>
	[JsonPropertyName("type")]
	public string? Type { get; init; }

	/// <summary>The user's time zone; empty for the server's.</summary>
	[JsonPropertyName("tz")]
	public string? TimeZone { get; init; }

	/// <summary>The user's language; empty for the browser's.</summary>
	[JsonPropertyName("lang")]
	public string? Language { get; init; }

	/// <summary>Whether the user is locked out after failed logins.</summary>
	[JsonPropertyName("locked-out")]
	public bool? LockedOut { get; init; }

	/// <summary>Whether the user's unfinished background searches restart when Splunk restarts.</summary>
	[JsonPropertyName("restart_background_jobs")]
	public bool? RestartBackgroundJobs { get; init; }

	/// <summary>When the user last logged in successfully.</summary>
	[JsonPropertyName("last_successful_login")]
	[JsonConverter(typeof(EpochSecondsConverter))]
	public DateTimeOffset? LastSuccessfulLogin { get; init; }

	/// <summary>The password, always masked (<c>********</c>).</summary>
	[JsonPropertyName("password")]
	public string? Password { get; init; }

	/// <summary>The user's Splunk Web theme, for example <c>default_system_theme</c>.</summary>
	[JsonPropertyName("theme")]
	public string? Theme { get; init; }
}
