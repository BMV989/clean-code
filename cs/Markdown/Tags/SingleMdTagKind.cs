using Markdown.Models;

namespace Markdown.Tags;

public class SingleMdTagKind(string mdTagKind, string htmlOpenTag, string htmlCloseTag) : IMdTagKind
{
    public string MdTag => mdTagKind;
    public string HtmlOpenTag => htmlOpenTag;
    public string HtmlCloseTag => htmlCloseTag;

    public SingleMdTagKind() : this(string.Empty, string.Empty, string.Empty)
    {
    }
    
    private bool IsValidTag(string text, int position) =>
        text.IsSubstring(position, MdTag)
        && (text.IsSubstring(position, Environment.NewLine, false) || position == 0);
    
    public bool TokenCanBeCreated(string text, int startIndex, int stopIndex) =>
        IsValidTag(text, startIndex) 
        && (text.IsSubstring(stopIndex, Environment.NewLine, false) || text.Length == stopIndex);

    public bool TryGetToken(string text, Tag openTag, List<Tag> closeTags, out Token token, out Tag closeTag)
    {
        var closeTagIndex = text.GetEndOfLinePosition(openTag.Position);
        
        if (TokenCanBeCreated(text, openTag.Position, closeTagIndex))
        {
            closeTag = null!;
            token = text.CreateToken(openTag.Position, closeTagIndex, this);
            return true;
        }

        closeTag = null!;
        token = null!;
        return false;

    }

    public string RemoveMdTags(string text) => text.Remove(0, MdTag.Length);

    public string InsertHtmlTags(string text) =>
        text
            .Insert(0, HtmlOpenTag)
            .Insert(text.EndsWith(Environment.NewLine)
                    ? text.Length - Environment.NewLine.Length
                    : text.Length, HtmlCloseTag);
}