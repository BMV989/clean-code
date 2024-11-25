namespace Markdown.Tags;

public class ItalicTag : PairTag
{
    public override string MdOpenTag => "_";
    public override string MdCloseTag => MdOpenTag;
    public override string HtmlTag => "em";
    public override IEnumerable<ITag> ForbiddenInside => [new BoldTag()];
}