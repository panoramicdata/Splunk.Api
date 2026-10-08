using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Access;

/// <summary>The settings of a user, shared by <see cref="UserCreateRequest"/> and <see cref="UserUpdateRequest"/>.</summary>
public abstract class UserSettings : SplunkFormRequest
{
	/// <summary>
	/// The roles to assign, sent once per role. On update, this replaces the user's roles. A new user needs at least one
	/// existing role unless <see cref="UserCreateRequest.CreateRole"/> is set.
	/// </summary>
	[JsonPropertyName("roles")]
	public IReadOnlyList<string>? Roles { get; init; }

	/// <summary>The user's full name.</summary>
	[JsonPropertyName("realname")]
	public string? RealName { get; init; }

	/// <summary>The user's email address.</summary>
	[JsonPropertyName("email")]
	public string? Email { get; init; }

	/// <summary>The app to open at login, overriding the roles' default app.</summary>
	[JsonPropertyName("defaultApp")]
	public string? DefaultApp { get; init; }

	/// <summary>The user's time zone.</summary>
	[JsonPropertyName("tz")]
	public string? TimeZone { get; init; }

	/// <summary>The user's language, for example <c>en-GB</c>.</summary>
	[JsonPropertyName("lang")]
	public string? Language { get; init; }

	/// <summary>Whether the user must change password at next login (<c>force-change-pass</c>).</summary>
	[JsonPropertyName("force-change-pass")]
	public bool? ForceChangePassword { get; init; }

	/// <summary>Whether the user's unfinished background searches restart when Splunk restarts.</summary>
	[JsonPropertyName("restart_background_jobs")]
	public bool? RestartBackgroundJobs { get; init; }

	/// <summary>Whether the user is locked out; send <see langword="false"/> to unlock.</summary>
	[JsonPropertyName("locked-out")]
	public bool? LockedOut { get; init; }
}
