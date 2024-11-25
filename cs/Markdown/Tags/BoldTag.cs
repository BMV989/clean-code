namespace Markdown.Tags;

public class BoldTag : PairTag
{
    public override string MdOpenTag => "__";
    public override string MdCloseTag => MdOpenTag;
    public override string HtmlTag => "strong";
}