using Refit;
using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;

namespace Splunk.Api.Interfaces;

/// <summary>
/// Health report settings: features, indicators and alert actions (<c>server/health-config</c>). Reading needs
/// <c>list_health</c>, changing <c>edit_health</c>.
/// </summary>
public interface IHealthConfig
{
	/// <summary>Lists the health report settings (<c>GET server/health-config</c>).</summary>
	/// <param name="options">Paging, filtering and sorting, or <see langword="null"/> for Splunk's defaults.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with one entry per stanza, for example <c>feature:iowait</c> or <c>alert_action:email</c>.</returns>
	[Get("services/server/health-config")]
	Task<SplunkFeed<HealthConfigStanza>> ListAsync([Query] ListOptions? options, CancellationToken cancellationToken);

	/// <summary>Configures an alert action (<c>POST server/health-config/alert_action:{actionName}</c>).</summary>
	/// <param name="actionName">The action, for example <c>email</c>, <c>pagerduty</c> or <c>webhook</c>.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated stanza.</returns>
	/// <remarks>The reference lists this as <c>server/health-config/{alert_action}</c>; the stanza is <c>alert_action:&lt;name&gt;</c>.</remarks>
	[Post("services/server/health-config/alert_action:{actionName}")]
	Task<SplunkFeed<HealthConfigStanza>> UpdateAlertActionAsync(string actionName, [Body] HealthAlertActionUpdateRequest request, CancellationToken cancellationToken);

	/// <summary>Changes a feature's settings and indicator thresholds (<c>POST server/health-config/feature:{featureName}</c>).</summary>
	/// <param name="featureName">The feature, for example <c>iowait</c> or <c>disk_space</c>.</param>
	/// <param name="request">The settings to change.</param>
	/// <param name="cancellationToken">A cancellation token.</param>
	/// <returns>A feed with the updated stanza.</returns>
	/// <remarks>The reference lists this as <c>server/health-config/{feature_name}</c>; the stanza is <c>feature:&lt;name&gt;</c>.</remarks>
	[Post("services/server/health-config/feature:{featureName}")]
	Task<SplunkFeed<HealthConfigStanza>> UpdateFeatureAsync(string featureName, [Body] HealthFeatureUpdateRequest request, CancellationToken cancellationToken);
}
