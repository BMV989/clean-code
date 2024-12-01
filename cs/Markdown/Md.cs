using Markdown.Tags;

namespace Markdown;

public static class Md
{
    private static readonly IEnumerable<IMdTagKind> Tags = [];
    public static string Render(string markdownText)
    {
        var root = new MdTokenizer(Tags).Tokenize(markdownText);
        return new HtmlMdConverter().Convert(root);
    }
}