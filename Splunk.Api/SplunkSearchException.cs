using Splunk.Api.Models;
using Splunk.Api.Models.Search;

namespace Splunk.Api;

/// <summary>
/// Raised by <see cref="SplunkSearch"/> when a search fails while running (for example an unknown function), as opposed
/// to a request Splunk rejects outright, which raises <see cref="SplunkApiException"/>.
/// </summary>
public sealed class SplunkSearchException : Exception
{
	/// <summary>Creates the exception.</summary>
	/// <param name="messages">Splunk's messages about the failure.</param>
	/// <param name="job">The failed job, when the search ran as a job.</param>
	public SplunkSearchException(IReadOnlyList<SplunkMessage> messages, SearchJob? job)
		: base(Describe(messages))
	{
		Messages = messages;
		Job = job;
	}

	/// <summary>The failed job, or <see langword="null"/> for an export.</summary>
	public SearchJob? Job { get; }

	/// <summary>Splunk's messages about the failure.</summary>
	public IReadOnlyList<SplunkMessage> Messages { get; }

	private static string Describe(IReadOnlyList<SplunkMessage> messages)
		=> messages.FirstOrDefault(m => IsError(m.Type))?.Text is { Length: > 0 } text ? text : "The search failed.";

	internal static bool IsError(string type)
		=> type.Equals("FATAL", StringComparison.OrdinalIgnoreCase) || type.Equals("ERROR", StringComparison.OrdinalIgnoreCase);
}
