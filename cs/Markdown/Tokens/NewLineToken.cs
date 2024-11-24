namespace Markdown.Tokens;

public class NewLineToken : IToken
{
    public string Value => "\n";
    public int Length => 1;
}