namespace Microsoft.CodeAnalysis.CSharp.PatternMatching;

internal class TokenListPattern
{
    private readonly string[] _tokens;

    public TokenListPattern(IEnumerable<string> tokens)
    {
        if (tokens == null)
            throw new ArgumentNullException(nameof(tokens));

        _tokens = tokens.ToArray();
    }

    public bool Test(SyntaxTokenList items, SemanticModel semanticModel)
    {
        if (items.Count != _tokens.Length)
            return false;

        for (var i = 0; i < items.Count; i++)
            if (items[i].Text != _tokens[i])
                return false;

        return true;
    }
}