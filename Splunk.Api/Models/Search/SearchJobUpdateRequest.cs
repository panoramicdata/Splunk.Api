namespace Splunk.Api.Models.Search;

/// <summary>
/// Sets custom properties on a search job (<c>POST search/jobs/{search_id}</c>). Each property is sent as
/// <c>custom.&lt;name&gt;</c> and read back from the job's content.
/// </summary>
public sealed class SearchJobUpdateRequest : SplunkFormRequest
{
	/// <summary>Creates a request that sets the given custom properties.</summary>
	/// <param name="customProperties">Property names (without the <c>custom.</c> prefix) and values.</param>
	public SearchJobUpdateRequest(IEnumerable<KeyValuePair<string, string>> customProperties)
	{
		ArgumentNullException.ThrowIfNull(customProperties);
		foreach (var (name, value) in customProperties)
		{
			AdditionalParameters["custom." + name] = value;
		}
	}
}
