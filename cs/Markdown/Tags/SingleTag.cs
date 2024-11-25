namespace Markdown.Tags;

public abstract class SingleTag : ITag
{
    public abstract string MdOpenTag { get; }
    public string? MdCloseTag => null;
    public abstract string HtmlTag { get; }
    public bool SelfClosingTag => true;
    public virtual IEnumerable<ITag> ForbiddenInside => [];
    
    public virtual bool IsOpenedCorrectly(ContextString ctx) => ctx.Left.Contains('\n');
    public bool IsClosedCorrectly(ContextString ctx) => true;
}