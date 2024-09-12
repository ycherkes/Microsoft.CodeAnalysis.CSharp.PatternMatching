using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.CodeAnalysis.CSharp.PatternMatching;

public class SingleStatementPattern : StatementPattern
{
    private readonly Action<StatementSyntax> _action;
    private readonly StatementPattern _statement;

    internal SingleStatementPattern(StatementPattern statement, Action<StatementSyntax> action)
    {
        _statement = statement;
        _action = action;
    }

    internal override bool Test(SyntaxNode node, SemanticModel semanticModel)
    {
        if (node is BlockSyntax block)
            return block.Statements.Count == 1 && Test(block.Statements[0], semanticModel);

        if (node is StatementSyntax statement)
            return _statement == null || _statement.Test(statement, semanticModel);

        return false;
    }

    internal override void RunCallback(SyntaxNode node, SemanticModel semanticModel)
    {
        if (node is BlockSyntax block)
        {
            RunCallback(block.Statements[0], semanticModel);
        }
        else
        {
            var statement = (StatementSyntax)node;

            _statement?.RunCallback(statement, semanticModel);
            _action?.Invoke(statement);
        }
    }
}