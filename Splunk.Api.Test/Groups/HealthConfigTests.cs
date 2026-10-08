using Splunk.Api.Models;
using Splunk.Api.Models.Introspection;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class HealthConfigTests
{
	// Captured from Splunk 10.6.0.5 (GET server/health-config), trimmed; alert settings filled in as health.conf documents them.
	private const string FeatureContent = """
		{
			"alert.disabled": "0",
			"alert.min_duration_sec": "60",
			"alert.threshold_color": "red",
			"display_name": "IOWait",
			"distributed_disabled": "1",
			"eai:acl": null,
			"friendly_description": "Shows if there's a delay in disk I/O requests.",
			"indicator:single_cpu__max_perc_last_3m:yellow": "5",
			"snooze_end_time": "0"
		}
		""";

	private const string ActionContent = """{"action.bcc":"c@example.com","action.cc":"b@example.com","action.to":"a@example.com","disabled":false,"eai:acl":null}""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(
			c => c.HealthConfig.ListAsync(new ListOptions { Count = 0 }, Calls.Token),
			HttpMethod.Get, "/services/server/health-config", "?count=0&output_mode=json", null);

	[Fact]
	public async Task UpdateAlertActionAsync_PostsToTheAlertActionStanza()
		=> await Calls.AssertAsync(
			c => c.HealthConfig.UpdateAlertActionAsync(
				"email",
				new HealthAlertActionUpdateRequest { To = "a@example.com", Cc = "b@example.com", Bcc = "c@example.com", IntegrationUrlOverride = "https://pd", Url = "https://hook", Disabled = false },
				Calls.Token),
			HttpMethod.Post, "/services/server/health-config/alert_action:email", Calls.JsonQuery,
			"action.to=a%40example.com&action.cc=b%40example.com&action.bcc=c%40example.com&action.integration_url_override=https%3A%2F%2Fpd"
			+ "&action.url=https%3A%2F%2Fhook&disabled=false");

	[Fact]
	public async Task UpdateFeatureAsync_PostsToTheFeatureStanza()
		=> await Calls.AssertAsync(
			c => c.HealthConfig.UpdateFeatureAsync(
				"iowait",
				new HealthFeatureUpdateRequest
				{
					AlertDisabled = true,
					AlertMinDurationSec = 60,
					AlertThresholdColor = "red",
					Disabled = false,
					DistributedDisabled = true,
					SnoozeEndTime = 1791500000,
					AdditionalParameters = { ["indicator:single_cpu__max_perc_last_3m:yellow"] = "10" }
				},
				Calls.Token),
			HttpMethod.Post, "/services/server/health-config/feature:iowait", Calls.JsonQuery,
			"alert.disabled=true&alert.min_duration_sec=60&alert.threshold_color=red&disabled=false&distributed_disabled=true"
			+ "&snooze_end_time=1791500000&indicator%3Asingle_cpu__max_perc_last_3m%3Ayellow=10");

	[Fact]
	public async Task ListAsync_MapsFeatureAndAlertActionStanzas()
	{
		var feed = await Calls.MapAsync(
			c => c.HealthConfig.ListAsync(null, Calls.Token),
			Feed.Of(("alert_action:email", ActionContent), ("feature:iowait", FeatureContent)));

		var action = feed.Entries[0].Content!;
		action.ActionTo.Should().Be("a@example.com");
		action.ActionCc.Should().Be("b@example.com");
		action.ActionBcc.Should().Be("c@example.com");
		action.Disabled.Should().BeFalse();
		var feature = feed.Entries[1].Content!;
		feature.AlertDisabled.Should().BeFalse();
		feature.AlertMinDurationSec.Should().Be(60);
		feature.AlertThresholdColor.Should().Be("red");
		feature.DisplayName.Should().Be("IOWait");
		feature.DistributedDisabled.Should().BeTrue();
		feature.FriendlyDescription.Should().Be("Shows if there's a delay in disk I/O requests.");
		feature.SnoozeEndTime.Should().Be(0);
		feature.AdditionalProperties["indicator:single_cpu__max_perc_last_3m:yellow"].GetString().Should().Be("5");
	}

	[Fact]
	public async Task UpdateFeatureAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.HealthConfig.UpdateFeatureAsync("nope", new HealthFeatureUpdateRequest(), Calls.Token), HttpStatusCode.NotFound);
}
