using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown;

public class Tokenizer(IEnumerable<ITag> tags)
{
    public List<IToken> Tokenize(string markdownText) => throw new NotImplementedException();
}