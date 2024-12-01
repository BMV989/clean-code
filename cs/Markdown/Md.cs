using Markdown.Tags;

namespace Markdown;

public static class Md
{
    private static IEnumerable<IMdTagKind> Tags
    {
        get
        {
            yield return new EscapeMdTagKind();
            yield return new SingleMdTagKind("#", "<h1>", "</h1>");
            yield return new PairMdTagKind("_", "<em>", "</em>");
            yield return new PairMdTagKind("__", "<strong>", "</strong>");
        }
    }

    private static IEnumerable<Func<Token, IEnumerable<Token>, bool>> TagRules
    {
        get
        {
            yield return IgnoreIntersectionBetweenPairTagsRule;
            yield return IgnorePairTagWhenParentPairTagHasGreaterLengthRule;
        }
    }

    public static string Render(string markdownText)
    {
        var root = new MdTokenizer(Tags.ToList(), TagRules).Tokenize(markdownText);
        return root.ConvertToHtml();
    }

    private static bool IgnorePairTagWhenParentPairTagHasGreaterLengthRule(Token tokenToCheck, 
        IEnumerable<Token> tokens) =>
        tokenToCheck.Tag is PairMdTagKind
        && tokens
            .Where(t => t != tokenToCheck && t.Tag is PairMdTagKind)
            .Any(parent => parent.IsChild(tokenToCheck)
                           && !(parent.Tag.MdTag.Length > tokenToCheck.Tag.MdTag.Length));


    private static bool IgnoreIntersectionBetweenPairTagsRule(Token tokenToCheck, IEnumerable<Token> tokens) =>
        tokenToCheck.Tag is PairMdTagKind
        && tokens
            .Where(t => t != tokenToCheck &&  t.Tag is PairMdTagKind)
            .Any(t => IsIntersectionBetween(tokenToCheck, t) 
                      || IsIntersectionBetween(t, tokenToCheck));

    private static bool IsIntersectionBetween(Token token, Token otherToken) =>
            token.Position > otherToken.Position 
            && token.Position < otherToken.Position + otherToken.Value.Length
            && token.Position + token.Value.Length > otherToken.Position + otherToken.Value.Length;
}