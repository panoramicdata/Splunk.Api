using Splunk.Api.Interfaces;

namespace Splunk.Api;

public sealed partial class SplunkClient
{
	/// <summary>Data model acceleration summaries (<c>admin/summarization</c>).</summary>
	public IDataModelSummaries DataModelSummaries => field ??= For<IDataModelSummaries>();

	/// <summary>Lookup table files (<c>data/lookup-table-files</c>).</summary>
	public ILookupTableFiles LookupTableFiles => field ??= For<ILookupTableFiles>();

	/// <summary>Calculated fields (<c>data/props/calcfields</c>).</summary>
	public ICalculatedFields CalculatedFields => field ??= For<ICalculatedFields>();

	/// <summary>Search-time field extractions (<c>data/props/extractions</c>).</summary>
	public IFieldExtractions FieldExtractions => field ??= For<IFieldExtractions>();

	/// <summary>Field aliases (<c>data/props/fieldaliases</c>).</summary>
	public IFieldAliases FieldAliases => field ??= For<IFieldAliases>();

	/// <summary>Automatic lookups (<c>data/props/lookups</c>).</summary>
	public IAutomaticLookups AutomaticLookups => field ??= For<IAutomaticLookups>();

	/// <summary>Sourcetype renames (<c>data/props/sourcetype-rename</c>).</summary>
	public ISourcetypeRenames SourcetypeRenames => field ??= For<ISourcetypeRenames>();

	/// <summary>Field transformations (<c>data/transforms/extractions</c>).</summary>
	public IFieldTransforms FieldTransforms => field ??= For<IFieldTransforms>();

	/// <summary>Lookup definitions (<c>data/transforms/lookups</c>).</summary>
	public ILookupDefinitions LookupDefinitions => field ??= For<ILookupDefinitions>();

	/// <summary>Log-to-metrics transformations (<c>data/transforms/metric-schema</c>).</summary>
	public IMetricSchemas MetricSchemas => field ??= For<IMetricSchemas>();

	/// <summary>StatsD dimension extractions (<c>data/transforms/statsdextractions</c>).</summary>
	public IStatsdExtractions StatsdExtractions => field ??= For<IStatsdExtractions>();

	/// <summary>The global banner (<c>data/ui/global-banner</c>).</summary>
	public IGlobalBanners GlobalBanners => field ??= For<IGlobalBanners>();

	/// <summary>Prebuilt dashboard panels (<c>data/ui/panels</c>).</summary>
	public IPanels Panels => field ??= For<IPanels>();

	/// <summary>Views and dashboards (<c>data/ui/views</c>).</summary>
	public IViews Views => field ??= For<IViews>();

	/// <summary>Data models and pivots (<c>datamodel/model</c>, <c>datamodel/pivot</c>).</summary>
	public IDataModels DataModels => field ??= For<IDataModels>();

	/// <summary>The directory of user-configurable objects (<c>directory</c>).</summary>
	public IDirectoryEntries DirectoryEntries => field ??= For<IDirectoryEntries>();

	/// <summary>Monitoring console bookmarks (<c>saved/bookmarks/monitoring_console</c>).</summary>
	public IMonitoringConsoleBookmarks MonitoringConsoleBookmarks => field ??= For<IMonitoringConsoleBookmarks>();

	/// <summary>Event types (<c>saved/eventtypes</c>).</summary>
	public IEventTypes EventTypes => field ??= For<IEventTypes>();

	/// <summary>Search field configurations and field value tags (<c>search/fields</c>).</summary>
	public ISearchFields SearchFields => field ??= For<ISearchFields>();

	/// <summary>Search-time tags (<c>search/tags</c>).</summary>
	public ISearchTags SearchTags => field ??= For<ISearchTags>();
}
