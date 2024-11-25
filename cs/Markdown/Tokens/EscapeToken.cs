namespace Markdown.Tokens;

public class EscapeToken : IToken
{
    public string Value => "\\";
}