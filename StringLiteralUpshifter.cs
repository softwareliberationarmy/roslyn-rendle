using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class StringLiteralUpshifter: CSharpSyntaxRewriter
{
    private readonly SemanticModel _model;

    public StringLiteralUpshifter(SemanticModel model)
    {
        _model = model;
    }

    public override SyntaxNode? VisitLiteralExpression(LiteralExpressionSyntax node)
    {
        if (_model.GetTypeInfo(node).Type is INamedTypeSymbol type)
        {
            if (type.Name == "String" && type.GetMembers(nameof(string.ToUpperInvariant)).FirstOrDefault() != null)
            {
                return SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        node,
                        SyntaxFactory.IdentifierName(nameof(string.ToUpperInvariant))));
            }
        }

        return base.VisitLiteralExpression(node);
    }
}