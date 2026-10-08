using Refit;

namespace Splunk.Api.Models.Spl2;

/// <summary>Query parameters for <c>GET orchestrator/v1/spl2/modules</c>.</summary>
public sealed class Spl2ModuleListOptions
{
	/// <summary>Only modules in this namespace (<c>namespace</c>), for example <c>apps.search</c>.</summary>
	[AliasAs("namespace")]
	public string? Namespace { get; init; }

	/// <summary>Search expressions on the response fields (<c>search</c>, repeated).</summary>
	[AliasAs("search")]
	[Query(CollectionFormat.Multi)]
	public IEnumerable<string>? Search { get; init; }

	/// <summary>Only modules containing this text in their description, display name or name (<c>filter</c>).</summary>
	[AliasAs("filter")]
	public string? Filter { get; init; }

	/// <summary>The most modules per page (<c>count</c>); Splunk's default is 1000.</summary>
	[AliasAs("count")]
	public int? Count { get; init; }

	/// <summary>The number of modules to skip (<c>offset</c>).</summary>
	[AliasAs("offset")]
	public int? Offset { get; init; }

	/// <summary>The properties to order by, comma-separated (<c>orderBy</c>).</summary>
	[AliasAs("orderBy")]
	public string? OrderBy { get; init; }

	/// <summary>Whether to return each module's definition (<c>includeDefinitions</c>).</summary>
	[AliasAs("includeDefinitions")]
	public bool? IncludeDefinitions { get; init; }

	/// <summary>Whether to return annotations (<c>include_annotations</c>).</summary>
	[AliasAs("include_annotations")]
	public bool? IncludeAnnotations { get; init; }

	/// <summary>Whether to leave out modules reserved for Edge Processor or Ingest Processor (<c>excludeReservedModules</c>).</summary>
	[AliasAs("excludeReservedModules")]
	public bool? ExcludeReservedModules { get; init; }

	/// <summary>Whether to return templates as well as, or instead of, modules (<c>templates</c>).</summary>
	[AliasAs("templates")]
	public Spl2TemplateFilter? Templates { get; init; }
}
