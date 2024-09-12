﻿using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.CSharp.PatternMatching;

public class AnySymbolPattern : ExpressionPattern
{
    private readonly Action<ExpressionSyntax, ISymbol> _action;

    internal AnySymbolPattern(Action<ExpressionSyntax, ISymbol> action)
    {
        _action = action;
    }

    internal override bool Test(SyntaxNode node, SemanticModel semanticModel)
    {
        if (semanticModel == null)
            throw new ArgumentNullException(nameof(semanticModel));

        if (!(node is ExpressionSyntax typed))
            return false;

        return semanticModel.TryGetSymbol(typed, out var _);
    }

    internal override void RunCallback(SyntaxNode node, SemanticModel semanticModel)
    {
        if (semanticModel == null)
            throw new ArgumentNullException(nameof(semanticModel));

        if (_action == null)
            return;

        var typed = (ExpressionSyntax)node;

        semanticModel.TryGetSymbol(typed, out var symbol);

        _action?.Invoke(typed, symbol);
    }
}