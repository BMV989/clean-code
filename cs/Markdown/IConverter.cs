using Markdown.Tokens;

namespace Markdown;

public interface IConverter
{
    string Convert(List<IToken> tokens);
}