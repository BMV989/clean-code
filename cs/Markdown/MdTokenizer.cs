using Markdown.Models;
using Markdown.Tags;

namespace Markdown;

public class MdTokenizer(List<IMdTagKind> tags, IEnumerable<Func<Token, IEnumerable<Token>, bool>> tagRules)
{
    private readonly Dictionary<string, IMdTagKind> availableTags = tags.ToDictionary(tag => tag.MdTag, tag => tag);
    private readonly List<Func<Token, IEnumerable<Token>, bool>> tagRules = tagRules.ToList();
    private readonly List<int> mdLenOfTagSignatures = tags
            .Select(tag => tag.MdTag.Length)
            .Distinct()
            .OrderDescending()
            .ToList();
        
    public Token Tokenize(string text)
    {
        var root = new Token(text);
        
        foreach (var line in GetLines(text))
        {
            var tokens = GetTokens(line.Value).OrderBy(t => t.Position).ToList();
            foreach (var token in tokens
                         .Where(t => tagRules.Select(rule => rule(t, tokens))
                             .All(result => !result))) line.AddToken(token);
            root.AddToken(line);
        }
        
        return root;
    }

    private static IEnumerable<Token> GetLines(string text)
    {
        var position = 0;
        foreach (var line in text.Split(Environment.NewLine))
        {
            yield return new Token(line, position, new SingleMdTagKind());
            position += line.Length + Environment.NewLine.Length;
        }
    }

    private IEnumerable<Token> GetTokens(string text)
    {
        var tags = GetTags(text).ToList();
        var escapeTokens = ParseEscapedTokens(text, tags).ToList();
        
        return ParseTokens(text, tags).Concat(escapeTokens);
    }

    private IEnumerable<Tag> GetTags(string text)
    {
        for (var pos = 0; pos < text.Length; pos++)
        {
            if (!TryGetTag(text, pos, out var tag)) continue;
            
            yield return new Tag(pos, tag);
            
            pos += tag.Length - 1;
        }
    }

    private bool TryGetTag(string text, int position, out IMdTagKind mdTag)
    {
        foreach (var mdLenOfTagSignature in mdLenOfTagSignatures)
        {
            if (position + mdLenOfTagSignature > text.Length || !availableTags
                    .TryGetValue(text.Substring(position, mdLenOfTagSignature), out var tag)) continue;

            mdTag = tag;
            return true;
        }

        mdTag = null!;
        return false;
    }

    private static IEnumerable<Token> ParseEscapedTokens(string text, List<Tag> tags)
    {
        for (var idx = 0; idx < tags.Count - 1; idx += 1)
        {
            if (tags[idx].TagKind is not EscapeMdTagKind) continue;
            
            var position = tags[idx].Position;
            tags.Remove(tags[idx]);

            if (tags[idx].Position - position == 1)
            {
                yield return text.CreateEscapeToken(tags[idx]);
                tags.Remove(tags[idx]);
            }

            idx -= 1;
        }
    }

    private static IEnumerable<Token> ParseTokens(string text, List<Tag> tags)
    {
        for (var idx = 0; idx < tags.Count; idx += 1)
        {
            if (!tags[idx].TagKind.TryGetToken(text, tags[idx], tags, out var token, 
                    out var closeToken)) continue;

            if (closeToken != null) tags.Remove(closeToken);
            
            yield return token;
            tags.RemoveAt(idx);
            
            idx -= 1;
        }
    }
}