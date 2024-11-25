namespace Markdown.Tags;

public class HeaderTag : SingleTag
{
    public override string MdOpenTag => "# ";
    public override string HtmlTag => "h1";
}