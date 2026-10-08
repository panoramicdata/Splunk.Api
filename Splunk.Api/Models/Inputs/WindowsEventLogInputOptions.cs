using Refit;

namespace Splunk.Api.Models.Inputs;

/// <summary>Options for reading one Windows event log collection.</summary>
public sealed class WindowsEventLogInputOptions
{
	/// <summary>For Splunk Web's internal use: the host being edited (<c>lookup_host</c>).</summary>
	[AliasAs("lookup_host")]
	public string? LookupHost { get; init; }
}
