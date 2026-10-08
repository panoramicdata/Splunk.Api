using System.Text.Json.Serialization;

namespace Splunk.Api.Models.Server;

/// <summary>The roles the server plays (<c>server/roles</c>).</summary>
public sealed class ServerRoles : SplunkContent
{
	/// <summary>The roles, for example <c>indexer</c>, <c>license_manager</c>, <c>kv_store</c>, <c>search_head</c>.</summary>
	[JsonPropertyName("role_list")]
	public IReadOnlyList<string> Roles { get; init; } = [];
}
