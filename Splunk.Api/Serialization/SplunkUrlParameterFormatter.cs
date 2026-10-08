using Refit;
using System.Reflection;

namespace Splunk.Api.Serialization;

/// <summary>
/// Formats query and path parameters the same way form fields are written: booleans as lowercase <c>true</c>/<c>false</c>,
/// enums by their wire name, dates and numbers in the invariant culture.
/// </summary>
internal sealed class SplunkUrlParameterFormatter : DefaultUrlParameterFormatter
{
	/// <inheritdoc />
	public override string? Format(object? parameterValue, ICustomAttributeProvider attributeProvider, Type type)
		=> parameterValue switch
		{
			null => null,
			bool or Enum or DateTimeOffset or DateTime or TimeSpan => SplunkFormEncoder.FormatScalar(parameterValue),
			_ => base.Format(parameterValue, attributeProvider, type)
		};
}
