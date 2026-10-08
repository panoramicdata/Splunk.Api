namespace Splunk.Api.Models.Introspection;

/// <summary>Changes an index's settings (<c>POST data/indexes/{name}</c>). Some settings take effect only after a restart.</summary>
public sealed class IndexUpdateRequest : IndexSettings;
