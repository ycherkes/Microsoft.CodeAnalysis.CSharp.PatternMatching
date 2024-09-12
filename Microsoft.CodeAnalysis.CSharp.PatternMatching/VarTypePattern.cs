using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.CSharp.PatternMatching;

public class VarTypePattern : TypePattern
{
    private readonly Action<TypeSyntax> _action;

    public VarTypePattern(Action<TypeSyntax> action)
    {
        _action = action;
    }

    internal override bool Test(SyntaxNode node, SemanticModel semanticModel)
    {
        return node is TypeSyntax typed && !typed.IsVar;
    }

    internal override void RunCallback(SyntaxNode node, SemanticModel semanticModel)
    {
        _action?.Invoke((TypeSyntax)node);
    }
}