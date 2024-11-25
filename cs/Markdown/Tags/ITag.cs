namespace Markdown.Tags;

public interface ITag
{
    string MdOpenTag { get; }
    string? MdCloseTag { get; }
    string HtmlTag { get; }
    bool  SelfClosingTag { get; }
    IEnumerable<ITag> ForbiddenInside { get; }
    
    bool IsOpenedCorrectly(ContextString ctx);
    bool IsClosedCorrectly(ContextString ctx);
}