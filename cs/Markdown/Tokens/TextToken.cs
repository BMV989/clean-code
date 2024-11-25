namespace Markdown.Tokens;

public class TextToken(string value) : IToken
{
    public string Value => value;
}