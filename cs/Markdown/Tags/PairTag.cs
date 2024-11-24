namespace Markdown.Tags;

public abstract class PairTag : ITag 
{
    public abstract string MdTag { get; }
    public abstract string HtmlTag { get; }

    public virtual bool IsOpenedCorrectly((char left, char right) contextChars) => contextChars.left != ' ';
    public virtual bool IsClosedCorrectly((char left, char right) contextChars) => contextChars.right != ' ';

    protected virtual IEnumerable<ITag> ForbiddenInside => [];
}