using Markdown.Tags;

namespace Markdown.Tokens;

public class TagToken(ITag tag, char left, char right) : IToken
{
    public ITag Tag => tag;
    public TagStatus Status { get; set; }
    // TODO: we should get value of tag by his status?
    public string Value => throw new NotImplementedException();
    public int Length => Tag.MdTag.Length;
    public (char left, char right) ContextChars => (left, right);
}