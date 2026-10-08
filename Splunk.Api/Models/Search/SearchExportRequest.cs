namespace Splunk.Api.Models.Search;

/// <summary>
/// Runs a search and streams its results as they become available (<c>POST search/v2/jobs/export</c>). Takes the same
/// parameters as creating a job.
/// </summary>
public sealed class SearchExportRequest : SearchJobParameters;
