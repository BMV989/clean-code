using Markdown.Tags;

namespace Markdown.Tokens;

public class TagToken(ITag tag, ContextString contextString) : IToken
{
    public ITag Tag => tag;
    public TagStatus Status { get; set; }
    // TODO: we should get value of tag by his status?
    public string Value => throw new NotImplementedException();
    public ContextString Context => contextString;
}