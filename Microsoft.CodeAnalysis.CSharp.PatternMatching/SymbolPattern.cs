using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.CSharp.PatternMatching;

public class SymbolPattern : ExpressionPattern
{
    private readonly Action<ExpressionSyntax> _action;
    private readonly ISymbol _symbol;

    public SymbolPattern(ISymbol symbol, Action<ExpressionSyntax> action)
    {
        _symbol = symbol;
        _action = action;
    }

    internal override bool Test(SyntaxNode node, SemanticModel semanticModel)
    {
        if (semanticModel == null)
            throw new ArgumentNullException(nameof(semanticModel));

        if (!(node is ExpressionSyntax typed))
            return false;

        if (!semanticModel.TryGetSymbol(typed, out var nodeSymbol))
            return false;

        return _symbol == null || _symbol.Equals(nodeSymbol, SymbolEqualityComparer.Default);
    }

    internal override void RunCallback(SyntaxNode node, SemanticModel semanticModel)
    {
        if (semanticModel == null)
            throw new ArgumentNullException(nameof(semanticModel));

        _action?.Invoke((ExpressionSyntax)node);
    }
}