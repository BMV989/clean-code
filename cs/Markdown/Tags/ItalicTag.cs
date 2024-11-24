namespace Markdown.Tags;

public class ItalicTag : PairTag, ITag
{
    public override string MdTag => "_";
    public override string HtmlTag => "<em>";

    protected override IEnumerable<ITag> ForbiddenInside => [new BoldTag()];
}