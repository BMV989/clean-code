namespace Markdown.Tokens;

public interface IToken
{
    string Value { get; }
    int Length { get; }
}