using Splunk.Api.Models;
using Splunk.Api.Serialization;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Splunk.Api.Test.Core;

public class FormEncoderTests
{
	private sealed class Everything : SplunkFormRequest
	{
		[JsonPropertyName("dotted.name")]
		public string? Text { get; init; }

		public int? CamelCaseNumber { get; init; }

		public bool? Flag { get; init; }

		public SortMode? Mode { get; init; }

		public IReadOnlyList<int?>? Numbers { get; init; }

		[JsonIgnore]
		public string Ignored { get; init; } = "ignored";

		public string NotPubliclyReadable { private get; init; } = "hidden";

		public string this[int index] => "indexer";

		public static string Static => "static";
	}

	private sealed class PlainObject
	{
		public string Name { get; init; } = "n";

		public double Ratio { get; init; } = 0.5;
	}

	/// <summary>The fields as <c>name=value</c> pairs joined by <c>&amp;</c>, unescaped.</summary>
	private static string Encode(object body) => string.Join('&', SplunkFormEncoder.Encode(body).Select(f => $"{f.Key}={f.Value}"));

	[Fact]
	public void FormRequest_IsEncodedPropertyByPropertyInDeclarationOrder()
		=> Encode(new Everything
		{
			Text = "a b",
			CamelCaseNumber = 3,
			Flag = false,
			Mode = SortMode.AlphabeticalCaseSensitive,
			Numbers = [1, null, 2],
			AdditionalParameters = { ["action.email"] = "1", ["empty"] = null }
		}).Should().Be("dotted.name=a b&camel_case_number=3&flag=false&mode=alpha_case&numbers=1&numbers=2&action.email=1&empty=");

	[Fact]
	public void NullAndEmptyValues_AreLeftOutOrSentEmpty()
		=> Encode(new Everything { Text = "", Numbers = [] }).Should().Be("dotted.name=");

	[Fact]
	public void AdditionalParameters_CollidingWithATypedField_AreRejected()
	{
		var act = () => Encode(new Everything { Flag = true, AdditionalParameters = { ["flag"] = "0" } });

		act.Should().Throw<ArgumentException>().WithMessage("The additional parameter 'flag' is also set by a typed property*");
	}

	[Fact]
	public void AdditionalParameters_MayUseTheNameOfAnUnsetTypedField()
		=> Encode(new Everything { AdditionalParameters = { ["flag"] = "0" } }).Should().Be("flag=0");

	[Fact]
	public void AnyObject_IsEncodedByItsProperties()
		=> Encode(new PlainObject()).Should().Be("name=n&ratio=0.5");

	[Fact]
	public void StringDictionaries_AreSentAsGiven()
	{
		Encode(new Dictionary<string, string?> { ["b"] = "2", ["a"] = null }).Should().Be("b=2&a=");
		Encode(new SortedDictionary<string, string> { ["z"] = "1" }).Should().Be("z=1");
		Encode(new List<KeyValuePair<string, string>> { new("dup", "1"), new("dup", "2") }).Should().Be("dup=1&dup=2");
	}

	[Fact]
	public void OtherDictionaries_AreEncodedEntryByEntry()
		=> Encode(new Dictionary<string, object?> { ["count"] = 5, ["list"] = new List<string> { "a", "b" }, ["none"] = null, ["on"] = true })
			.Should().Be("count=5&list=a&list=b&none=&on=true");

	[Fact]
	public void NonStringKeys_AreFormattedAsScalars()
		=> Encode(new Dictionary<SortDirection, int> { [SortDirection.Descending] = 1 }).Should().Be("desc=1");

	[Fact]
	public void Encode_RequiresABody()
	{
		var act = () => Encode(null!);

		act.Should().Throw<ArgumentNullException>();
	}

	public static TheoryData<object, string> Scalars => new()
	{
		{ "text", "text" },
		{ true, "true" },
		{ false, "false" },
		{ SortDirection.Ascending, "asc" },
		{ (SortDirection)9, "9" },
		{ 42, "42" },
		{ -7L, "-7" },
		{ 1.25, "1.25" },
		{ 1e21, "1E+21" },
		{ 0.1m, "0.1" },
		{ new DateTimeOffset(2026, 10, 8, 14, 18, 12, 345, TimeSpan.FromHours(1)), "2026-10-08T14:18:12.345+01:00" },
		{ new DateTime(2026, 10, 8, 13, 18, 12, 5, DateTimeKind.Utc), "2026-10-08T13:18:12.005" },
		{ TimeSpan.FromSeconds(90.9), "90" },
		{ Guid.Empty, "00000000-0000-0000-0000-000000000000" },
		{ 'c', "c" },
		{ new Uri("https://x.test/a b"), "https://x.test/a b" }
	};

	[Theory]
	[MemberData(nameof(Scalars))]
	public void FormatScalar_UsesSplunksSpelling_InTheInvariantCulture(object value, string expected)
	{
		var previous = CultureInfo.CurrentCulture;
		CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-DE");
		try
		{
			SplunkFormEncoder.FormatScalar(value).Should().Be(expected);
		}
		finally
		{
			CultureInfo.CurrentCulture = previous;
		}
	}

	private sealed class NullText
	{
		public override string? ToString() => null;
	}

	[Fact]
	public void FormatScalar_NullToString_IsEmpty()
		=> SplunkFormEncoder.FormatScalar(new NullText()).Should().BeEmpty();

	[Fact]
	public void Models_AclAndMove_UseTheirWireNames()
	{
		Encode(new AclUpdateRequest { Sharing = "app", Owner = "nobody", Read = ["*"] }).Should().Be("sharing=app&owner=nobody&perms.read=*");
		Encode(new MoveRequest { App = "search", User = "nobody" }).Should().Be("app=search&user=nobody");
	}
}
