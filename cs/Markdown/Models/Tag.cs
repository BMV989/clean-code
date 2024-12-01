using Markdown.Tags;

namespace Markdown.Models;

public record Tag(int Position, IMdTagKind TagKind);