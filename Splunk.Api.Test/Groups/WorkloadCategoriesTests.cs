using Splunk.Api.Models;
using Splunk.Api.Models.WorkloadManagement;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class WorkloadCategoriesTests
{
	// Captured from Splunk 10.6.0.5 (GET workloads/categories, entry "search").
	private const string SearchContent = """
		{"cpu_allocated_percent":70,"cpu_shares":0,"cpu_weight":70,"cpu_weight_sum":100,"eai:acl":null,"mem_allocated_percent":70,"mem_limit":"0","mem_weight":70}
		""";

	[Fact]
	public async Task ListAsync_SendsGet()
		=> await Calls.AssertAsync(c => c.WorkloadCategories.ListAsync(null, Calls.Token), HttpMethod.Get, "/services/workloads/categories", Calls.JsonQuery, null);

	[Fact]
	public async Task UpdateAsync_PostsTheWeightsToTheCategory()
		=> await Calls.AssertAsync(
			c => c.WorkloadCategories.UpdateAsync("search", new WorkloadWeightsRequest { CpuWeight = 60, MemoryWeight = 80 }, Calls.Token),
			HttpMethod.Post, "/services/workloads/categories/search", Calls.JsonQuery, "cpu_weight=60&mem_weight=80");

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var feed = await Calls.MapAsync(c => c.WorkloadCategories.ListAsync(new ListOptions(), Calls.Token), Feed.Of("search", SearchContent));

		var category = feed.Entries.Should().ContainSingle().Subject.Content!;
		category.CpuAllocatedPercent.Should().Be(70);
		category.CpuShares.Should().Be(0);
		category.CpuWeight.Should().Be(70);
		category.CpuWeightSum.Should().Be(100);
		category.MemoryAllocatedPercent.Should().Be(70);
		category.MemoryLimit.Should().Be("0");
		category.MemoryWeight.Should().Be(70);
	}

	[Fact]
	public async Task UpdateAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.WorkloadCategories.UpdateAsync("nope", new WorkloadWeightsRequest(), Calls.Token), HttpStatusCode.NotFound);
}
