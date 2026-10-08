using Splunk.Api.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Splunk.Api.Test.Core;

/// <summary>A JSON <c>null</c> for a collection property keeps the model's <c>[]</c> default.</summary>
public class CollectionDefaultsTests
{
	private sealed class Lists
	{
		[JsonPropertyName("list")]
		public IReadOnlyList<string> List { get; init; } = [];

		[JsonPropertyName("ilist")]
		public IList<int> MutableList { get; init; } = [];

		[JsonPropertyName("sequence")]
		public IEnumerable<string> Sequence { get; init; } = [];

		[JsonPropertyName("array")]
		public string[] Array { get; init; } = [];

		[JsonPropertyName("map")]
		public IReadOnlyDictionary<string, string> Map { get; init; } = new Dictionary<string, string>();

		[JsonPropertyName("unset")]
		public IReadOnlyList<string>? Unset { get; init; }

		[JsonPropertyName("text")]
		public string? Text { get; init; } = "initial";

		[JsonPropertyName("computed")]
		public IReadOnlyList<string> Computed => List;
	}

	private static Lists Read(string json) => JsonSerializer.Deserialize<Lists>(json, SplunkJson.Options)!;

	[Fact]
	public void Null_KeepsTheInitialiser()
	{
		var lists = Read("""{"list":null,"ilist":null,"sequence":null,"array":null,"map":null,"unset":null,"computed":null}""");

		lists.List.Should().BeEmpty();
		lists.MutableList.Should().BeEmpty();
		lists.Sequence.Should().BeEmpty();
		lists.Array.Should().BeEmpty();
		lists.Map.Should().BeEmpty();
		lists.Unset.Should().BeNull();
	}

	[Fact]
	public void Values_AreStillRead()
	{
		var lists = Read("""{"list":["a"],"ilist":[1],"sequence":["s"],"array":["x"],"map":{"k":"v"},"unset":["u"]}""");

		lists.List.Should().Equal("a");
		lists.MutableList.Should().Equal(1);
		lists.Sequence.Should().Equal("s");
		lists.Array.Should().Equal("x");
		lists.Map.Should().Equal(new Dictionary<string, string> { ["k"] = "v" });
		lists.Unset.Should().Equal("u");
	}

	[Fact]
	public void NullStrings_AreStillNull()
		=> Read("""{"text":null}""").Text.Should().BeNull();

	[Fact]
	public void SharedModels_KeepTheirDefaults()
	{
		var feed = JsonSerializer.Deserialize<SplunkFeed<SplunkDynamicContent>>(
			"""{"links":null,"entry":[{"name":"e","links":null,"acl":{"perms":{"read":null,"write":null}}}],"messages":null}""",
			SplunkJson.Options)!;

		feed.Links.Should().BeEmpty();
		feed.Messages.Should().BeEmpty();
		var entry = feed.Entries.Should().ContainSingle().Subject;
		entry.Links.Should().BeEmpty();
		entry.Acl!.Permissions!.Read.Should().BeEmpty();
		entry.Acl.Permissions.Write.Should().BeEmpty();
	}
}
