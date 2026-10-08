using Microsoft.Extensions.Logging;

namespace Splunk.Api.Handlers;

/// <summary>Source-generated log messages. Only methods and paths are logged, never query strings or credentials.</summary>
internal static partial class Log
{
	[LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "Splunk {Method} {Path} (attempt {Attempt})")]
	public static partial void Sending(ILogger logger, HttpMethod method, string path, int attempt);

	[LoggerMessage(EventId = 2, Level = LogLevel.Warning, Message = "Splunk returned {Status} for {Method} {Path}; retrying in {Delay}")]
	public static partial void Retrying(ILogger logger, int status, HttpMethod method, string path, TimeSpan delay);

	[LoggerMessage(EventId = 3, Level = LogLevel.Warning, Message = "Splunk connection failed ({Error}) for {Method} {Path}; retrying in {Delay}")]
	public static partial void RetryingConnection(ILogger logger, HttpRequestError error, HttpMethod method, string path, TimeSpan delay);
}
