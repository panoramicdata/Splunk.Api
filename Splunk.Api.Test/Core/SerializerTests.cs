using Splunk.Api.Models;
using Splunk.Api.Serialization;
using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;

namespace Splunk.Api.Test.Core;

public class SerializerTests
{
	private readonly SplunkContentSerializer _serializer = new();

	private sealed class Named
	{
		[JsonPropertyName("wire_name")]
		public string? Renamed { get; init; }

		public string? Plain { get; init; }
	}

	[Fact]
	public async Task ToHttpContent_JsonBody_IsJson()
	{
		using var content = _serializer.ToHttpContent(new JsonBody<Named>(new Named { Renamed = "x" }));

		content.Headers.ContentType!.ToString().Should().Be("application/json");
		(await content.ReadAsStringAsync(TestContext.Current.CancellationToken)).Should().Be("""{"wire_name":"x"}""");
	}

	[Fact]
	public async Task ToHttpContent_OtherBodies_AreBufferedFormFields()
	{
		using var content = _serializer.ToHttpContent(new MoveRequest { App = "a&b", User = "nobody" });

		content.Should().BeOfType<FormUrlEncodedContent>();
		(await content.ReadAsStringAsync(TestContext.Current.CancellationToken)).Should().Be("app=a%26b&user=nobody");
	}

	[Fact]
	public void ToHttpContent_RequiresABody()
	{
		var act = () => _serializer.ToHttpContent<object?>(null);

		act.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public async Task FromHttpContent_ReadsWithSplunkJsonOptions()
	{
		using var content = new StringContent("""{"WIRE_NAME":"x","plain":5}""", Encoding.UTF8, "application/json");

		var value = await _serializer.FromHttpContentAsync<Named>(content, TestContext.Current.CancellationToken);

		value!.Renamed.Should().Be("x");
		value.Plain.Should().Be("5");
	}

	[Theory]
	[InlineData(nameof(Named.Renamed), "wire_name")]
	[InlineData(nameof(Named.Plain), null)]
	public void GetFieldNameForProperty_UsesJsonPropertyName(string property, string? expected)
		=> _serializer.GetFieldNameForProperty(typeof(Named).GetProperty(property)!).Should().Be(expected);

	public static TheoryData<object?, string?> UrlValues => new()
	{
		{ null, null },
		{ true, "true" },
		{ SortMode.Numeric, "num" },
		{ new DateTimeOffset(2026, 1, 2, 3, 4, 5, TimeSpan.Zero), "2026-01-02T03:04:05.000+00:00" },
		{ new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Unspecified), "2026-01-02T03:04:05.000" },
		{ TimeSpan.FromMinutes(2), "120" },
		{ 1.5, "1.5" },
		{ 12, "12" },
		{ "text", "text" }
	};

	[Theory]
	[MemberData(nameof(UrlValues))]
	public void UrlParameterFormatter_MatchesFormFields(object? value, string? expected)
	{
		var previous = CultureInfo.CurrentCulture;
		CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
		try
		{
			new SplunkUrlParameterFormatter().Format(value, typeof(SerializerTests), value?.GetType() ?? typeof(object)).Should().Be(expected);
		}
		finally
		{
			CultureInfo.CurrentCulture = previous;
		}
	}
}
