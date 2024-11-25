using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public class MdTokenizer(IEnumerable<ITag> tags)
{
    public List<IToken> Tokenize(string markdownText) => throw new NotImplementedException();
}