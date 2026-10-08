using Splunk.Api.Models.Introspection;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class HealthTests
{
	// Captured from Splunk 10.6.0.5 (GET server/health/splunkd/details), trimmed to two feature categories.
	private const string DetailsContent = """
		{
			"eai:acl": null,
			"features": {
				"Index Processor": { "features": { "Buckets": { "health": "green" }, "Disk Space": { "health": "green" } }, "health": "green" },
				"Resource Usage": {
					"features": {
						"IOWait": {
							"health": "yellow",
							"messages": null,
							"reasons": { "yellow": { "1": { "indicator": "single_cpu__max_perc_last_3m", "reason": "Maximum per-cpu iowait reached yellow threshold of 5" } } }
						}
					},
					"health": "yellow"
				},
				"Workload Management": { "disabled": true, "health": "green" }
			},
			"health": "yellow"
		}
		""";

	[Fact]
	public async Task GetDeploymentAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Health.GetDeploymentAsync(Calls.Token), HttpMethod.Get, "/services/server/health/deployment", Calls.JsonQuery, null);

	[Fact]
	public async Task GetDeploymentDetailsAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Health.GetDeploymentDetailsAsync(Calls.Token), HttpMethod.Get, "/services/server/health/deployment/details", Calls.JsonQuery, null);

	[Fact]
	public async Task GetSplunkdAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Health.GetSplunkdAsync(Calls.Token), HttpMethod.Get, "/services/server/health/splunkd", Calls.JsonQuery, null);

	[Fact]
	public async Task GetSplunkdDetailsAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.Health.GetSplunkdDetailsAsync(Calls.Token), HttpMethod.Get, "/services/server/health/splunkd/details", Calls.JsonQuery, null);

	[Fact]
	public async Task GetDeploymentAsync_MapsTheSummary()
	{
		// Captured from Splunk 10.6.0.5.
		var feed = await Calls.MapAsync(c => c.Health.GetDeploymentAsync(Calls.Token), Feed.Of("deployment", """{"disabled":true,"eai:acl":null,"health":"green"}"""));

		var report = feed.Entries.Should().ContainSingle().Subject.Content!;
		report.Health.Should().Be(HealthColor.Green);
		report.Disabled.Should().BeTrue();
		report.Features.Should().BeEmpty();
	}

	[Fact]
	public async Task GetSplunkdDetailsAsync_MapsTheFeatureTree()
	{
		var feed = await Calls.MapAsync(c => c.Health.GetSplunkdDetailsAsync(Calls.Token), Feed.Of("splunkd", DetailsContent));

		var report = feed.Entries.Should().ContainSingle().Subject.Content!;
		report.Health.Should().Be(HealthColor.Yellow);
		report.Features.Keys.Should().BeEquivalentTo("Index Processor", "Resource Usage", "Workload Management");
		report.Features["Index Processor"].Features["Disk Space"].Health.Should().Be(HealthColor.Green);
		report.Features["Workload Management"].Disabled.Should().BeTrue();
		report.Features["Workload Management"].Features.Should().BeEmpty();
		var iowait = report.Features["Resource Usage"].Features["IOWait"];
		iowait.Health.Should().Be(HealthColor.Yellow);
		iowait.Messages.Should().BeNull();
		iowait.Reasons!.Value.GetProperty("yellow").GetProperty("1").GetProperty("indicator").GetString().Should().Be("single_cpu__max_perc_last_3m");
	}

	[Theory]
	[InlineData("red", HealthColor.Red)]
	[InlineData("purple", HealthColor.Unknown)]
	public async Task GetSplunkdAsync_MapsTheColor(string wire, HealthColor expected)
	{
		var feed = await Calls.MapAsync(c => c.Health.GetSplunkdAsync(Calls.Token), Feed.Of("splunkd", $$"""{"health":"{{wire}}"}"""));

		feed.Entries[0].Content!.Health.Should().Be(expected);
	}

	[Fact]
	public async Task GetSplunkdAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.Health.GetSplunkdAsync(Calls.Token), HttpStatusCode.Forbidden);
}
