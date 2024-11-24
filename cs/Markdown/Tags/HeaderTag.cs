namespace Markdown.Tags;

public class HeaderTag : SingleTag, ITag
{
    public override string MdTag => "#";
    public override string HtmlTag => "<h1>";
}