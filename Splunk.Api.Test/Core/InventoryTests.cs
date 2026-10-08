using Refit;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Splunk.Api.Test.Core;

/// <summary>
/// Keeps <c>docs/endpoints/*.md</c> (one row per documented Splunk 10.6 REST operation) and the Refit interfaces in step:
/// every filled row names real interface methods whose verb and path match the row, and a real test; every Refit
/// operation is listed; and every category not named in <c>docs/pending-categories.txt</c> is fully implemented.
/// </summary>
public partial class InventoryTests
{
	private const int DocumentedOperations = 656;

	private static readonly string RepoRoot = FindRepoRoot();
	private static readonly List<Row> Rows = ParseRows();
	private static readonly HashSet<string> PendingCategories = ReadPendingCategories();
	private static readonly List<Operation> Operations = FindOperations();

	public sealed record Row(string File, int Line, string Category, string Method, string Path, string[] ClientMethods, string[] Tests)
	{
		public bool IsDeprecated => Path.EndsWith("(deprecated)", StringComparison.Ordinal);

		public bool IsComplete => ClientMethods.Length > 0 && Tests.Length > 0;

		public bool IsStarted => ClientMethods.Length > 0 || Tests.Length > 0;

		public string NormalizedPath => Normalize(Path.Replace("(deprecated)", string.Empty, StringComparison.Ordinal).Trim());

		public override string ToString() => $"{File}:{Line} {Method} {Path}";
	}

	public sealed record Operation(string Interface, string Name, string Method, string NormalizedPath)
	{
		public string ClientMethod => $"{Interface}.{Name}";

		public override string ToString() => $"{ClientMethod} ({Method} {NormalizedPath})";
	}

	[Fact]
	public void Docs_HaveEveryDocumentedOperation() =>
		Rows.Should().HaveCount(DocumentedOperations, "docs/endpoints inventories every operation in the Splunk 10.6 REST API reference");

	[Fact]
	public void PendingCategories_AreCategoriesInTheDocs()
	{
		var categories = Rows.Select(r => r.Category).ToHashSet(StringComparer.Ordinal);

		AssertNone(PendingCategories.Where(c => !categories.Contains(c)), "every line of docs/pending-categories.txt must name a docs/endpoints file");
	}

	[Fact]
	public void NonPendingCategories_HaveEveryRowImplemented()
		=> AssertNone(
			Rows.Where(r => !r.IsDeprecated && !PendingCategories.Contains(r.Category) && !r.IsComplete).Select(r => r.ToString()),
			"a category not in docs/pending-categories.txt must fill Client method and Test on every non-deprecated row");

	[Fact]
	public void StartedRows_HaveBothClientMethodAndTest()
		=> AssertNone(Rows.Where(r => r.IsStarted && !r.IsComplete).Select(r => r.ToString()), "a row must fill both Client method and Test, or neither");

	[Fact]
	public void DeprecatedRows_AreNotImplemented()
		=> AssertNone(Rows.Where(r => r.IsDeprecated && r.IsStarted).Select(r => r.ToString()), "deprecated operations are not implemented");

	[Fact]
	public void ClientMethods_ExistAndMatchTheirRow()
	{
		var byName = Operations.ToLookup(o => o.ClientMethod, StringComparer.Ordinal);
		var offending = Rows
			.SelectMany(r => r.ClientMethods.Select(m => (Row: r, Name: m)))
			.Where(x => !byName[x.Name].Any(o => o.Method == x.Row.Method && o.NormalizedPath == x.Row.NormalizedPath))
			.Select(x => $"{x.Row}: {x.Name} " + (byName[x.Name].Any() ? $"is {string.Join(", ", byName[x.Name])}" : "does not exist"));

		AssertNone(offending, "each Client method must be a Refit method in Splunk.Api whose verb and path (placeholder names ignored) match the row");
	}

	[Fact]
	public void Tests_Exist()
	{
		var tests = typeof(InventoryTests).Assembly.GetTypes()
			.SelectMany(t => t.GetMethods().Where(m => m.GetCustomAttributes<FactAttribute>(true).Any()).Select(m => $"{t.Name}.{m.Name}"))
			.ToHashSet(StringComparer.Ordinal);

		AssertNone(Rows.SelectMany(r => r.Tests.Where(t => !tests.Contains(t)).Select(t => $"{r}: {t}")), "each Test must be TestClass.TestMethod of a [Fact] or [Theory] in Splunk.Api.Test");
	}

	[Fact]
	public void EveryRefitOperation_IsListed()
	{
		var listed = Rows.SelectMany(r => r.ClientMethods).ToHashSet(StringComparer.Ordinal);

		Operations.Should().NotBeEmpty();
		AssertNone(Operations.Where(o => !listed.Contains(o.ClientMethod)).Select(o => o.ToString()), "every Refit operation must appear in a docs/endpoints row");
	}

	private static void AssertNone(IEnumerable<string> offending, string because)
	{
		var list = offending.ToList();
		list.Should().BeEmpty("{0}; found {1}:{2}{3}", because, list.Count, Environment.NewLine, string.Join(Environment.NewLine, list));
	}

	internal static string Normalize(string path)
	{
		var withoutQuery = path.Split('?')[0].Trim('/');
		var withoutPrefix = withoutQuery.StartsWith("services/", StringComparison.Ordinal) ? withoutQuery["services/".Length..] : withoutQuery;
		return PlaceholderRegex().Replace(withoutPrefix, "{}");
	}

	[GeneratedRegex(@"\{[^}]*\}")]
	private static partial Regex PlaceholderRegex();

	[GeneratedRegex(@"^\| (GET|POST|DELETE|PUT) \| `([^`]+)` \|([^|]*)\|([^|]*)\|$")]
	private static partial Regex RowRegex();

	private static List<Row> ParseRows()
	{
		var rows = new List<Row>();
		foreach (var file in Directory.GetFiles(Path.Combine(RepoRoot, "docs", "endpoints"), "*.md").Order(StringComparer.Ordinal))
		{
			var lines = File.ReadAllLines(file);
			for (var i = 0; i < lines.Length; i++)
			{
				if (RowRegex().Match(lines[i].TrimEnd()) is { Success: true } match)
				{
					rows.Add(new Row(
						Path.GetFileName(file),
						i + 1,
						Path.GetFileNameWithoutExtension(file),
						match.Groups[1].Value,
						match.Groups[2].Value,
						SplitCell(match.Groups[3].Value),
						SplitCell(match.Groups[4].Value)));
				}
			}
		}

		return rows;
	}

	private static string[] SplitCell(string cell)
		=> [.. cell.Replace("`", string.Empty, StringComparison.Ordinal).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];

	private static HashSet<string> ReadPendingCategories()
		=> [.. File.ReadAllLines(Path.Combine(RepoRoot, "docs", "pending-categories.txt"))
			.Select(l => l.Trim())
			.Where(l => l.Length > 0 && !l.StartsWith('#'))];

	private static List<Operation> FindOperations()
		=> [.. typeof(SplunkClient).Assembly.GetExportedTypes()
			.Where(t => t.IsInterface)
			.SelectMany(t => t.GetMethods()
				.Select(m => (Method: m, Attribute: m.GetCustomAttribute<HttpMethodAttribute>()))
				.Where(x => x.Attribute is not null)
				.Select(x => new Operation(t.Name, x.Method.Name, x.Attribute!.Method.Method, Normalize(x.Attribute.Path))))];

	private static string FindRepoRoot()
	{
		var directory = new DirectoryInfo(AppContext.BaseDirectory);
		while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Splunk.Api.slnx")))
		{
			directory = directory.Parent;
		}

		return directory?.FullName ?? throw new InvalidOperationException("Could not find the repository root (Splunk.Api.slnx).");
	}
}
