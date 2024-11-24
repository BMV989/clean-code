namespace Markdown.Tokens;

public class TextToken(string value) : IToken
{
    public string Value => value;
    public int Length => Value.Length;
}