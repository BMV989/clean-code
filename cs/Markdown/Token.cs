using System.Text;
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
        var parent = children.FirstOrDefault(token => token.IsChild(child));
        
        if (parent != null)
        {
            parent.AddToken(child);
            child.Position -= parent.Position + parent.Tag.MdTag.Length;
        }
        else children.Add(child);
    }

    public string ConvertToHtml()
    {
        var sb = new StringBuilder(Tag.RemoveMdTags(Value));
        foreach (var child in children.OrderByDescending(token => token.Position))
        {
            sb.Remove(child.Position, child.Value.Length);
            sb.Insert(child.Position, child.ConvertToHtml());
        }

        return Tag.InsertHtmlTags(sb.ToString());
    }

    public bool IsChild(Token child) =>
        child.Position >= Position 
        && child.Position < Position + Value.Length 
        && child.Position + child.Value.Length <= Position + Value.Length;
}