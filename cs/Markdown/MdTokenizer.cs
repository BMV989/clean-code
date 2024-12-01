using Markdown.Tags;

namespace Markdown;

public class MdTokenizer(IEnumerable<IMdTagKind> tags)
{
    public Token Tokenize(string markdownText) => throw new NotImplementedException();
}