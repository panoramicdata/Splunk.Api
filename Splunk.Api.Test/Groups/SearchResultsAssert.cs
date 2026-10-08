using Splunk.Api.Models.Search;

namespace Splunk.Api.Test.Groups;

/// <summary>The assertions for <see cref="SearchJson.Results"/>, shared by every endpoint that returns that shape.</summary>
internal static class SearchResultsAssert
{
	public static void AssertResults(SearchResults results)
	{
		results.Preview.Should().BeFalse();
		results.InitOffset.Should().Be(0);
		results.PostProcessCount.Should().Be(3);
		results.Messages.Should().ContainSingle().Which.Type.Should().Be("WARN");
		results.Fields.Select(f => f.Name).Should().Equal("_time", "mv", "x", "host");
		results.Fields[2].Type.Should().Be("str");
		results.Fields[0].Type.Should().BeNull();
		results.Fields[3].GroupByRank.Should().Be(0);
		results.Fields[3].AdditionalProperties["summary.count"].GetString().Should().Be("5");
		results.Highlighted.Should().ContainKey("0");
		results.Results.Should().HaveCount(2);
		var first = results.Results[0];
		first.FieldNames.Should().Equal("_time", "mv", "x", "host");
		first["x"].Should().Be("2123633747");
		first.GetValues("mv").Should().Equal("a", "b");
		first.IsMultivalue("mv").Should().BeTrue();
		first.Time.Should().Be(new DateTimeOffset(2026, 10, 8, 13, 47, 15, TimeSpan.Zero));
		var second = results.Results[1];
		second.GetValues("n").Should().BeEmpty();
		second.Contains("n").Should().BeTrue();
		second.GetString("n").Should().BeNull();
		second.IsMultivalue("mv").Should().BeFalse();
		second["mv"].Should().Be("c");
	}
}
