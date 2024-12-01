using Markdown.Models;

namespace Markdown.Tags;

public class PairMdTagKind(string mdTag, string htmlOpenTag, string htmlCloseTag) : IMdTagKind
{
    public string MdTag => mdTag;
    public string HtmlOpenTag => htmlOpenTag;
    public string HtmlCloseTag => htmlCloseTag;

    private bool IsValidTag(string text, int position) =>
        text.IsSubstring(position, MdTag)
        && text.IsSubstring(position, char.IsDigit, false) != true
        && text.IsSubstring(position + MdTag.Length, char.IsDigit, false) != true;
    
    public bool TokenCanBeCreated(string text, int startIndex, int stopIndex)
    {
        if(!IsValidTag(text, startIndex) || !IsValidTag(text, stopIndex - MdTag.Length)) return false;
        
        var value = text.Substring(startIndex, stopIndex - startIndex);
        if (value.Split(' ').Length == 1) return value.Length > MdTag.Length * 2;
        
        return value.Split(Environment.NewLine).Length == 1 
               && text.IsSubstring(startIndex, char.IsWhiteSpace, false) != false 
               && text.IsSubstring(stopIndex, char.IsWhiteSpace) != false;
    }


    public bool TryGetToken(string text, Tag openTag, List<Tag> closeTags, out Token token, out Tag closeTag)
    {
        foreach (var tag in closeTags.Where(tag => openTag != tag &&
                                                   openTag.TagKind == tag.TagKind &&
                                                   openTag.Position <= tag.Position &&
                                                   openTag.TagKind.TokenCanBeCreated(text, openTag.Position, 
                                                       tag.Position + tag.TagKind.Length)))
        {
            closeTag = tag;
            token = text.CreateToken(openTag.Position, 
                tag.Position + tag.TagKind.Length, openTag.TagKind);
            return true;
        }

        closeTag = null!;
        token = null!;
        return false;
    }

    public string RemoveMdTags(string text) =>
        text
            .Remove(0, MdTag.Length)
            .Remove(text.Length - MdTag.Length);

    public string InsertHtmlTags(string text) =>
        text
            .Insert(0, HtmlOpenTag)
            .Insert(text.Length, HtmlCloseTag);
}
