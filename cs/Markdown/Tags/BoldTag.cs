namespace Markdown.Tags;

public class BoldTag : PairTag, ITag
{
    public override string MdTag => "__";
    public override string HtmlTag => "<strong>";
}