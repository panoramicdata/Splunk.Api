using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api.Interfaces;

/// <summary>Triggered alerts (<c>alerts/fired_alerts</c>), for alerts that track them (<c>alert.track</c>).</summary>
public interface IFiredAlerts
{
	/// <summary>Lists the alerts with unexpired triggered instances (<c>GET alerts/fired_alerts</c>).</summary>
	/// <param name="options">Paging and filtering; <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per alert, plus <c>-</c> totalling them.</returns>
	[Get("services/alerts/fired_alerts")]
	Task<SplunkFeed<FiredAlertSummary>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Lists an alert's unexpired triggered instances (<c>GET alerts/fired_alerts/{name}</c>).</summary>
	/// <param name="name">The alert (saved search) name; <c>-</c> for every alert.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per triggered instance.</returns>
	[Get("services/alerts/fired_alerts/{name}")]
	Task<SplunkFeed<FiredAlert>> GetAsync(string name, CancellationToken cancellationToken);

	/// <summary>Deletes a triggered alert instance (<c>DELETE alerts/fired_alerts/{name}</c>).</summary>
	/// <param name="name">The instance name, as listed by <see cref="GetAsync"/>.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A task that completes when the instance is deleted.</returns>
	/// <remarks>
	/// Despite the reference, the name is a triggered instance's (<c>scheduler__…_&lt;trigger time&gt;</c>), not the
	/// alert's: an alert name fails with <c>Unexpected pattern for alert_id</c>. So does an instance whose job was not run
	/// by the scheduler (one dispatched with <c>trigger_actions</c>).
	/// </remarks>
	[Delete("services/alerts/fired_alerts/{name}")]
	Task DeleteAsync(string name, CancellationToken cancellationToken);
}
