namespace Splunk.Api.Models.Knowledge;

/// <summary>
/// A sourcetype rename in <c>props.conf</c> (<c>data/props/sourcetype-rename</c>): the stanza is the original sourcetype and
/// <see cref="PropsEntry.Value"/> the new name.
/// </summary>
public sealed class SourcetypeRename : PropsEntry;
