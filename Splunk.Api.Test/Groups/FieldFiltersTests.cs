using Splunk.Api.Models.Access;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class FieldFiltersTests
{
	// Shaped after the reference (field filters affect every search on the shared test instance).
	private const string FilterJson = """
		{
			"entry": [
				{
					"name": "mask_ssn",
					"content": {
						"action.field": "ssn",
						"action.operator": "sha256()",
						"description": "Hash social security numbers",
						"index": "main,hr",
						"limit.key": "sourcetype",
						"limit.value": "\"hr:payroll\"",
						"roleExemptions": ["admin"]
					}
				}
			]
		}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> (await EndpointRequests.SendAsync((c, ct) => c.FieldFilters.ListAsync(null, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/authorization/fieldfilters");

	[Fact]
	public async Task CreateAsync_PostsTheFilter()
		=> (await EndpointRequests.SendAsync((c, ct) => c.FieldFilters.CreateAsync(
			new FieldFilterCreateRequest
			{
				Name = "mask_ssn",
				ActionField = "ssn",
				ActionOperator = "null()",
				Description = "d",
				Index = "main",
				LimitKey = "host",
				LimitValue = "\"web01\"",
				RoleExemptions = "admin"
			},
			ct)))
			.ShouldBeEndpointRequest(
				HttpMethod.Post,
				"/services/authorization/fieldfilters",
				"name=mask_ssn&action.field=ssn&action.operator=null%28%29&description=d&index=main&limit.key=host&limit.value=%22web01%22&roleExemptions=admin");

	[Fact]
	public async Task GetAsync_SendsGetForTheName()
		=> (await EndpointRequests.SendAsync((c, ct) => c.FieldFilters.GetAsync("mask_ssn", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Get, "/services/authorization/fieldfilters/mask_ssn");

	[Fact]
	public async Task UpdateAsync_PostsTheChanges()
		=> (await EndpointRequests.SendAsync((c, ct) => c.FieldFilters.UpdateAsync("mask_ssn", new FieldFilterUpdateRequest { ActionOperator = "sha512()" }, ct)))
			.ShouldBeEndpointRequest(HttpMethod.Post, "/services/authorization/fieldfilters/mask_ssn", "action.operator=sha512%28%29");

	[Fact]
	public async Task DeleteAsync_SendsDelete()
		=> (await EndpointRequests.SendAsync((c, ct) => c.FieldFilters.DeleteAsync("mask_ssn", ct)))
			.ShouldBeEndpointRequest(HttpMethod.Delete, "/services/authorization/fieldfilters/mask_ssn");

	[Fact]
	public async Task Content_MapsEveryModelledField()
	{
		var feed = await EndpointRequests.ReadAsync((c, ct) => c.FieldFilters.GetAsync("mask_ssn", ct), FilterJson);

		var filter = feed.Entries.Should().ContainSingle().Subject.Content!;
		filter.ActionField.Should().Be("ssn");
		filter.ActionOperator.Should().Be("sha256()");
		filter.Description.Should().Be("Hash social security numbers");
		filter.Index.Should().Be("main,hr");
		filter.LimitKey.Should().Be("sourcetype");
		filter.LimitValue.Should().Be("\"hr:payroll\"");
		filter.RoleExemptions.Should().Be("""["admin"]""");
	}

	[Fact]
	public Task Error_RaisesSplunkApiException()
		=> EndpointRequests.ShouldRaiseSplunkErrorAsync((c, ct) => c.FieldFilters.GetAsync("missing", ct));
}
