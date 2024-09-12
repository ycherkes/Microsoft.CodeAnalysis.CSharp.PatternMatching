namespace Microsoft.CodeAnalysis.CSharp.PatternMatching;

public class NullExpressionPattern : ExpressionPattern
{
    private readonly Action _action;

    internal NullExpressionPattern(Action action)
    {
        _action = action;
    }

    internal override bool Test(SyntaxNode node, SemanticModel semanticModel)
    {
        return node == null;
    }

    internal override void RunCallback(SyntaxNode node, SemanticModel semanticModel)
    {
        _action?.Invoke();
    }
}