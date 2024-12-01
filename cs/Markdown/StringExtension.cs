using Markdown.Models;
using Markdown.Tags;

namespace Markdown;

public static class StringExtension
{
    public static bool IsSubstring(this string text, int position, string value, bool isForward = true)
    {
        if (isForward ? position + value.Length > text.Length : position - value.Length < 0) return false;

        var substring = isForward
            ? text.Substring(position, value.Length)
            : text.Substring(position - value.Length, value.Length);

        return substring == value;
    }
    
    public static bool? IsSubstring(this string text, int position, Predicate<char> predicate, bool isForward = true)
    {
        if (isForward ? position + 1 > text.Length : position - 1 < 0) return null;

        position = isForward ? position : position - 1;
        return predicate(text[position]);
    }
    
    public static Token CreateToken(this string text, int startIndex, int stopIndex, IMdTagKind tag)
    {
        var value = text.Substring(startIndex, stopIndex - startIndex);
        return new Token(value, startIndex, tag);
    }
    
    public static int GetEndOfLinePosition(this string text, int startIndex = 0)
    {
        var newLinePosition = text.IndexOf(Environment.NewLine, startIndex, StringComparison.Ordinal); 
        return newLinePosition != -1 ? newLinePosition + Environment.NewLine.Length : text.Length;
    }
    
    public static Token CreateEscapeToken(this string text, Tag escapeTag)
    {
        var value = text.Substring(escapeTag.Position - 1, escapeTag.TagKind.Length);
        return new Token(value, escapeTag.Position - 1, new EscapeMdTagKind());
    }
}