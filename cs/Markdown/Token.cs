using Markdown.Tags;

namespace Markdown;

public class Token(string value, int position, IMdTagKind mdTagKind)
{
    private readonly List<Token> children = [];
    
    public string Value => value;
    public IMdTagKind Tag => mdTagKind;
    public int Position { get; private set; } = position;

    public Token(string value) : this(value, value.Length, new SingleMdTagKind())
    {
    }

    public void AddToken(Token child)
    {
        var parent = children.FirstOrDefault(t =>  t.IsChild(child));
        
        if (parent == null) children.Add(child);
        else
        {
            parent.AddToken(child);
            child.Position -= parent.Position + parent.Tag.MdTag.Length;
        }
    }

    public bool IsChild(Token child) => 
        child.Position >= Position &&
        child.Position < Position + Value.Length &&
        child.Position + child.Value.Length <= Position + Value.Length;
}