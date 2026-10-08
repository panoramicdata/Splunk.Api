using Splunk.Api.Models.Inputs;
using Splunk.Api.Test.Support;

namespace Splunk.Api.Test.Groups;

public class IngestRulesetsTests
{
	private const string Path = "/services/data/ingest/rulesets";

	// Captured from Splunk 10.6.0.5 after creating a ruleset (it sends no paging details); a route rule added.
	private const string RulesetJson = """
		{
			"links": {}, "origin": "https://splunk.test:8089/services/data/ingest/rulesets/", "updated": "2026-10-08T14:07:18+00:00",
			"generator": { "build": "86587d4e3b27", "version": "10.6.0.5" },
			"entry": [
				{
					"name": "drop_debug", "id": "https://splunk.test:8089/services/data/ingest/rulesets/drop_debug",
					"updated": "1970-01-01T00:00:00+00:00", "links": { "alternate": "/services/data/ingest/rulesets/drop_debug" },
					"content": {
						"name": "drop_debug", "description": "Drop debug", "sourcetype": "app_log",
						"rules": [
							{ "name": "r1", "action": "filter", "cond": { "type": "regex", "field": "_raw", "match": "DEBUG" } },
							{ "name": "r2", "action": "route", "dest": "rfs:archive" }
						]
					}
				}
			]
		}
		""";

	private static readonly IngestRule FilterRule = new()
	{
		Name = "r1",
		Action = "filter",
		Condition = new IngestRuleCondition { Type = "regex", Field = "_raw", Match = "DEBUG" }
	};

	[Fact]
	public async Task ListAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IngestRulesets.ListAsync(ct))).ShouldBeGet(Path);

	[Fact]
	public async Task CreateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IngestRulesets.CreateAsync(
			new IngestRulesetCreateRequest { Name = "drop_debug", Sourcetype = "app_log", Description = "Drop debug", Rules = [FilterRule] }, ct)))
			.ShouldBePost(Path, "name=drop_debug&sourcetype=app_log&description=Drop+debug&rules=%5B%7B%22name%22%3A%22r1%22%2C%22action%22%3A%22filter%22%2C%22cond%22%3A%7B%22type%22%3A%22regex%22%2C%22field%22%3A%22_raw%22%2C%22match%22%3A%22DEBUG%22%7D%7D%5D");

	[Fact]
	public async Task GetAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IngestRulesets.GetAsync("drop_debug", ct))).ShouldBeGet(Path + "/drop_debug");

	[Fact]
	public async Task UpdateAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IngestRulesets.UpdateAsync("drop_debug", new IngestRulesetUpdateRequest { Sourcetype = "app_log" }, ct)))
			.ShouldBePost(Path + "/drop_debug", "sourcetype=app_log");

	[Fact]
	public async Task PublishAsync_SendsExactRequest()
		=> (await InputsTestKit.CaptureAsync((c, ct) => c.IngestRulesets.PublishAsync(ct))).ShouldBePost(Path + "/publish", null);

	[Fact]
	public async Task GetAsync_MapsEveryModelledField()
	{
		var ruleset = (await InputsTestKit.MapEntryAsync((c, ct) => c.IngestRulesets.GetAsync("drop_debug", ct), RulesetJson)).Content!;

		ruleset.Name.Should().Be("drop_debug");
		ruleset.Description.Should().Be("Drop debug");
		ruleset.Sourcetype.Should().Be("app_log");
		ruleset.Rules.Should().HaveCount(2);
		ruleset.Rules[0].Name.Should().Be("r1");
		ruleset.Rules[0].Action.Should().Be("filter");
		ruleset.Rules[0].Condition!.Type.Should().Be("regex");
		ruleset.Rules[0].Condition!.Field.Should().Be("_raw");
		ruleset.Rules[0].Condition!.Match.Should().Be("DEBUG");
		ruleset.Rules[1].Condition.Should().BeNull();
		ruleset.Rules[1].AdditionalProperties["dest"].GetString().Should().Be("rfs:archive");
	}

	[Fact]
	public Task GetAsync_Error_RaisesSplunkApiException()
		=> InputsTestKit.ShouldRaiseNotFoundAsync((c, ct) => c.IngestRulesets.GetAsync("nope", ct));
}
