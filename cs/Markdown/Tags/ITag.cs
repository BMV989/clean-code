namespace Markdown.Tags;

public interface ITag
{
    string MdTag { get; }
    string HtmlTag { get; }
    
    bool IsOpenedCorrectly((char left, char right) contextChars);
    bool IsClosedCorrectly((char left, char right) contextChars);
}