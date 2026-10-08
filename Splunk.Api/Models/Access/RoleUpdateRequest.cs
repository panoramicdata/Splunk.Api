namespace Splunk.Api.Models.Access;

/// <summary>Changes a role (<c>POST authorization/roles/{name}</c>).</summary>
/// <remarks>
/// The reference also lists <c>imported_*</c> parameters for this POST; a live Splunk 10.6 computes those from the
/// imported roles and does not accept them, so set <see cref="RoleSettings.ImportedRoles"/> instead.
/// </remarks>
public sealed class RoleUpdateRequest : RoleSettings;
