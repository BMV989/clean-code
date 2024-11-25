namespace Markdown.Tags;

public abstract class PairTag : ITag 
{
    public abstract string MdOpenTag { get; }
    public abstract string MdCloseTag { get; }
    public abstract string HtmlTag { get; }
    public bool SelfClosingTag => false;
    public virtual IEnumerable<ITag> ForbiddenInside => [];

    public virtual bool IsOpenedCorrectly(ContextString ctx) => ctx.Left.First() != ' ';
    public virtual bool IsClosedCorrectly(ContextString ctx) => ctx.Right.First() != ' ';
}