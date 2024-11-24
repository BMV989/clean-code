using Markdown.Tokens;

namespace Markdown;

public interface IMdConverter
{
    string Convert(List<IToken> tokens);
}