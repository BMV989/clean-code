namespace Markdown.Tags;

public abstract class SingleTag : ITag
{
    public abstract string MdTag { get; }
    public abstract string HtmlTag { get; }
    
    public virtual bool IsOpenedCorrectly((char left, char right) contextChars) => contextChars.left  == '\n';
    public bool IsClosedCorrectly((char left, char right) contextChars) => true;
}