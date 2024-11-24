using Markdown.Tags;

namespace Markdown;

public static class Md
{
    private static readonly IEnumerable<ITag> Tags = [new BoldTag(), new HeaderTag(), new ItalicTag()];
    public static string Render(string markdownText)
    {
        var tokens = new Tokenizer(Tags).Tokenize(markdownText);
        return new HtmlConverter().Convert(tokens);
    }
}