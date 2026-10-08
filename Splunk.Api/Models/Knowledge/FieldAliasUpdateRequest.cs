namespace Splunk.Api.Models.Knowledge;

/// <summary>Replaces the aliases of a field alias group (<c>POST data/props/fieldaliases/{name}</c>).</summary>
/// <remarks>Splunk replaces the group's aliases with those sent: aliases left out are removed.</remarks>
public sealed class FieldAliasUpdateRequest : FieldAliasSettings
{
}
