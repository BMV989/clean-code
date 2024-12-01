using Markdown.Models;

namespace Markdown.Tags;

public interface IMdTagKind
{
    public string MdTag { get; }
    public string HtmlOpenTag { get; }
    public string HtmlCloseTag { get; }
    public int Length => MdTag.Length;

    public bool TokenCanBeCreated(string text, int startIndex, int stopIndex);
    public bool TryGetToken(string text, Tag openTag, List<Tag> closeTags, out Token token, out Tag? closeTag);
    public string RemoveMdTags(string text);
    public string InsertHtmlTags(string text);
}