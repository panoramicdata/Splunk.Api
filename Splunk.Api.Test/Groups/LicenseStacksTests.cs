using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class LicenseStacksTests
{
	// Captured from Splunk Enterprise 10.6.0.5 (GET licenser/stacks/free), trimmed; host replaced.
	private const string StackJson = """
		{
			"links": {},
			"origin": "https://splunk.test:8089/services/licenser/stacks",
			"entry": [
				{
					"name": "free",
					"author": "system",
					"content": {
						"cle_active": 0,
						"eai:acl": null,
						"is_unlimited": false,
						"label": "Splunk Free",
						"max_retention_size": 0,
						"max_violations": 3,
						"quota": 524288000,
						"type": "free",
						"window_period": 30
					}
				}
			],
			"paging": { "total": 1, "perPage": 30, "offset": 0 },
			"messages": []
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LicenseStacks.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/licenser/stacks");

	[Fact]
	public async Task GetAsync_SendsGetForTheId()
		=> (await EndpointRequests.SendAsync((c, ct) => c.LicenseStacks.GetAsync("free", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/licenser/stacks/free");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.LicenseStacks.GetAsync("free", ct), StackJson);

		var stack = feed.Entries.Should().ContainSingle().Subject.Content!;
		stack.ConditionalEnforcementActive.Should().BeFalse();
		stack.IsUnlimited.Should().BeFalse();
		stack.Label.Should().Be("Splunk Free");
		stack.MaxRetentionSize.Should().Be(0);
		stack.MaxViolations.Should().Be(3);
		stack.Quota.Should().Be(524288000);
		stack.Type.Should().Be("free");
		stack.WindowPeriod.Should().Be(30);
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.LicenseStacks.GetAsync("missing", ct));
}
