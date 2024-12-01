using Markdown.Models;

namespace Markdown.Tags;

public class EscapeMdTagKind : IMdTagKind
{
    public string MdTag => "\\";
    public string HtmlOpenTag => string.Empty;
    public string HtmlCloseTag => string.Empty;
    
    public bool TokenCanBeCreated(string text, int startIndex, int stopIndex) =>  text.IsSubstring(startIndex, MdTag);

    public bool TryGetToken(string text, Tag openTag, List<Tag> closeTags, out Token token, out Tag closeTag)
    {
        var openTagIndex = closeTags.IndexOf(openTag); 
        var escapedTag = openTagIndex + 1 > closeTags.Count 
            ? null 
            : closeTags[openTagIndex + 1];
        
        if (escapedTag != null)
        {
            closeTag = escapedTag;
            token = text.CreateToken(openTag.Position, 
                escapedTag.Position + escapedTag.TagKind.Length, this);
            return true;
        }
        
        closeTag = null!;
        token = null!;
        return false;
    }

    public string RemoveMdTags(string text) => text.Remove(0, MdTag.Length);

    public string InsertHtmlTags(string text) => text;
}