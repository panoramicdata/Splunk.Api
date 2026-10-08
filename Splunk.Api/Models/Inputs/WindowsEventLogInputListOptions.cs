using Refit;

namespace Splunk.Api.Models.Inputs;

/// <summary>Options for listing Windows event log collections.</summary>
public sealed class WindowsEventLogInputListOptions : ListOptions
{
	/// <summary>For Splunk Web's internal use: the host being edited (<c>lookup_host</c>).</summary>
	[AliasAs("lookup_host")]
	public string? LookupHost { get; init; }
}
