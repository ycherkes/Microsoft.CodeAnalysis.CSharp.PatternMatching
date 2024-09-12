﻿namespace Microsoft.CodeAnalysis.CSharp.PatternMatching;

internal class NodeListPattern<T>
    where T : PatternNode
{
    private readonly T[] _nodes;

    internal NodeListPattern(IEnumerable<T> nodes)
    {
        if (nodes == null)
            throw new ArgumentNullException(nameof(nodes));

        _nodes = nodes.ToArray();
    }

    public bool Test<TNode>(SyntaxList<TNode> items, SemanticModel semanticModel)
        where TNode : SyntaxNode
    {
        if (items.Count == _nodes.Length)
        {
            for (var i = 0; i < items.Count; i++)
                if (!_nodes[i].Test(items[i], semanticModel))
                    return false;

            return true;
        }

        return false;
    }

    public void RunCallback<TNode>(SyntaxList<TNode> items, SemanticModel semanticModel)
        where TNode : SyntaxNode
    {
        for (var i = 0; i < items.Count; i++) _nodes[i].RunCallback(items[i], semanticModel);
    }

    public bool Test<TNode>(SeparatedSyntaxList<TNode> items, SemanticModel semanticModel)
        where TNode : SyntaxNode
    {
        if (items.Count == _nodes.Length)
        {
            for (var i = 0; i < items.Count; i++)
                if (!_nodes[i].Test(items[i], semanticModel))
                    return false;

            return true;
        }

        return false;
    }

    public void RunCallback<TNode>(SeparatedSyntaxList<TNode> items, SemanticModel semanticModel)
        where TNode : SyntaxNode
    {
        for (var i = 0; i < items.Count; i++) _nodes[i].RunCallback(items[i], semanticModel);
    }
}