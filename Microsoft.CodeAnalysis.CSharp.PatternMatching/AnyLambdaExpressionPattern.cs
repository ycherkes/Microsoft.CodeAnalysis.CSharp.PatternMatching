using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.CSharp.PatternMatching;

public class AnyLambdaExpressionPattern : LambdaExpressionPattern
{
    private readonly Action<LambdaExpressionSyntax> _action;
    private readonly ParameterListPattern _parameterList;

    internal AnyLambdaExpressionPattern(PatternNode body, ParameterListPattern parameterList,
        Action<LambdaExpressionSyntax> action)
        : base(body)
    {
        _parameterList = parameterList;
        _action = action;
    }

    internal override bool Test(SyntaxNode node, SemanticModel semanticModel)
    {
        if (!base.Test(node, semanticModel))
            return false;

        if (node is ParenthesizedLambdaExpressionSyntax parenthesized)
        {
            if (_parameterList != null && !_parameterList.Test(parenthesized.ParameterList, semanticModel))
                return false;
        }
        else if (node is SimpleLambdaExpressionSyntax simple)
        {
            if (_parameterList != null && !_parameterList.Test(
                    SyntaxFactory.ParameterList(
                        SyntaxFactory.SeparatedList(new[] { simple.Parameter })
                    ),
                    semanticModel
                ))
                return false;
        }
        else
        {
            return false;
        }

        return true;
    }

    internal override void RunCallback(SyntaxNode node, SemanticModel semanticModel)
    {
        if (node is ParenthesizedLambdaExpressionSyntax parenthesized)
            _parameterList?.RunCallback(parenthesized.ParameterList, semanticModel);
        else
            _parameterList?.RunCallback(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SeparatedList(new[] { ((SimpleLambdaExpressionSyntax)node).Parameter })
                ),
                semanticModel
            );

        _action?.Invoke((LambdaExpressionSyntax)node);
    }
}