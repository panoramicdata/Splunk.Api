using Splunk.Api.Models.WorkloadManagement;
using Splunk.Api.Test.Support.Platform;
using System.Net;

namespace Splunk.Api.Test.Groups;

public class WorkloadRulesTests
{
	// The live test instance has no rules; this content uses the keys the 10.6 reference documents.
	private const string RuleContent = """
		{"action":"move","eai:acl":null,"order":"2","predicate":"app=search AND role=power","user_message":"","workload_pool":"fast"}
		""";

	[Fact]
	public async Task ListAsync_SendsTheRuleType()
		=> await Calls.AssertAsync(
			c => c.WorkloadRules.ListAsync(new WorkloadRuleListOptions { WorkloadRuleType = "search_filter", Count = 0 }, Calls.Token),
			HttpMethod.Get, "/services/workloads/rules", "?workload_rule_type=search_filter&count=0&output_mode=json", null);

	[Fact]
	public async Task CreateAsync_PostsEveryField()
		=> await Calls.AssertAsync(
			c => c.WorkloadRules.CreateAsync(
				new WorkloadRuleCreateRequest
				{
					Name = "power_users",
					Predicate = "app=search AND role=power",
					Action = "move",
					WorkloadPool = "fast",
					Order = 2,
					WorkloadRuleType = "search_filter",
					AdditionalParameters = { ["schedule"] = "always_on" }
				},
				Calls.Token),
			HttpMethod.Post, "/services/workloads/rules", Calls.JsonQuery,
			"name=power_users&predicate=app%3Dsearch+AND+role%3Dpower&action=move&workload_pool=fast&order=2&workload_rule_type=search_filter&schedule=always_on");

	[Fact]
	public async Task DeleteAsync_SendsDeleteForTheRule()
		=> await Calls.AssertAsync(
			c => c.WorkloadRules.DeleteAsync("power_users", new WorkloadRuleDeleteOptions { WorkloadRuleType = "search_filter" }, Calls.Token),
			HttpMethod.Delete, "/services/workloads/rules/power_users", "?workload_rule_type=search_filter&output_mode=json", null);

	[Fact]
	public async Task ListAsync_MapsEveryModelledField()
	{
		var feed = await Calls.MapAsync(c => c.WorkloadRules.ListAsync(null, Calls.Token), Feed.Of("power_users", RuleContent));

		var rule = feed.Entries.Should().ContainSingle().Subject.Content!;
		rule.Action.Should().Be("move");
		rule.Order.Should().Be(2);
		rule.Predicate.Should().Be("app=search AND role=power");
		rule.UserMessage.Should().BeEmpty();
		rule.WorkloadPool.Should().Be("fast");
	}

	[Fact]
	public async Task DeleteAsync_Error_RaisesSplunkApiException()
		=> await Calls.AssertErrorAsync(c => c.WorkloadRules.DeleteAsync("missing", null, Calls.Token), HttpStatusCode.NotFound);
}
