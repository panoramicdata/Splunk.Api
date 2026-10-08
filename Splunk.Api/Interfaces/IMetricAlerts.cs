using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>Streaming metric alerts (<c>alerts/metric_alerts</c>). Needs the <c>metric_alerts</c> capability.</summary>
public interface IMetricAlerts
{
	/// <summary>Lists the metric alerts (<c>GET alerts/metric_alerts</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed of metric alerts.</returns>
	[Get("services/alerts/metric_alerts")]
	Task<SplunkFeed<MetricAlert>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Creates a metric alert (<c>POST alerts/metric_alerts</c>).</summary>
	/// <param name="request">The name, condition, indexes and settings.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the new alert.</returns>
	[Post("services/alerts/metric_alerts")]
	Task<SplunkFeed<MetricAlert>> CreateAsync([Body] MetricAlertCreateRequest request, CancellationToken cancellationToken);

	/// <summary>Gets a metric alert (<c>GET alerts/metric_alerts/{alert_name}</c>).</summary>
	/// <param name="alertName">The alert name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry.</returns>
	[Get("services/alerts/metric_alerts/{alertName}")]
	Task<SplunkFeed<MetricAlert>> GetAsync(string alertName, CancellationToken cancellationToken);

	/// <summary>Changes, enables or disables a metric alert (<c>POST alerts/metric_alerts/{alert_name}</c>).</summary>
	/// <param name="alertName">The alert name.</param>
	/// <param name="request">The settings to change; set <see cref="MetricAlertSettings.Disabled"/> to enable or disable it.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry, the changed alert.</returns>
	[Post("services/alerts/metric_alerts/{alertName}")]
	Task<SplunkFeed<MetricAlert>> UpdateAsync(string alertName, [Body] MetricAlertUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Deletes a metric alert (<c>DELETE alerts/metric_alerts/{alert_name}</c>).</summary>
	/// <param name="alertName">The alert name.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the alert is deleted.</returns>
	[Delete("services/alerts/metric_alerts/{alertName}")]
	Task DeleteAsync(string alertName, CancellationToken cancellationToken);
}
