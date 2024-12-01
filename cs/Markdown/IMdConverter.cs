namespace Markdown;

public interface IMdConverter
{
    string Convert(Token root);
}