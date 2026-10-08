using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Splunk.Api.Models.Search;

/// <summary>
/// Reads a JSON export stream (<see cref="Interfaces.ISearchExport.ExportAsync"/>) incrementally: Splunk writes one JSON
/// object per line as results become available.
/// </summary>
public static class SearchExportReader
{
	/// <summary>Reads each record of the stream as it arrives. The caller still owns (and disposes) the stream.</summary>
	/// <param name="stream">The export response body.</param>
	/// <param name="cancellationToken">Stops reading.</param>
	/// <returns>The records, in stream order.</returns>
	public static async IAsyncEnumerable<SearchExportRecord> ReadAsync(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(stream);
		using var reader = new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 16 * 1024, leaveOpen: true);
		while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
		{
			if (!string.IsNullOrWhiteSpace(line))
			{
				yield return JsonSerializer.Deserialize<SearchExportRecord>(line, SplunkJson.Options)
					?? throw new JsonException("The export stream contained a null record.");
			}
		}
	}
}
