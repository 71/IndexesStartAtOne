using System.Threading;
using Cometary;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/// <summary>
///   Defines a <see cref="CompilationEditor"/> that replaces indexes
///   in index expressions, offseting them by a given value.
/// </summary>
internal sealed class IndexReplacer : CompilationEditor
{
    private readonly LiteralExpressionSyntax startAtSyntax;

    internal IndexReplacer(int startAt)
    {
        startAtSyntax = SyntaxFactory.LiteralExpression(
            SyntaxKind.NumericLiteralExpression,
            SyntaxFactory.Literal(startAt));
    }

    /// <inheritdoc />
    protected override void Initialize(CSharpCompilation compilation, CancellationToken cancellationToken)
    {
        this.DefineFeatureDependency(nameof(IOperation));
        this.EditSyntaxTree(EditSyntaxTree);
    }

    private CSharpSyntaxTree EditSyntaxTree(CSharpSyntaxTree tree, CSharpCompilation compilation, CancellationToken token)
    {
        // The whole compilation is edited at once instead of going through each node independently,
        // because syntax trees cannot change if we want to use their semantic model.

        SemanticModel semanticModel = compilation.GetSemanticModel(tree, true);
        SyntaxNode root             = tree.GetRoot(token);

        Rewriter rewriter = new Rewriter(semanticModel, startAtSyntax);

        return (CSharpSyntaxTree)tree.WithRoot(rewriter.Visit(root));
    }

    private sealed class Rewriter : CSharpSyntaxRewriter
    {
        private readonly SemanticModel semanticModel;
        private readonly LiteralExpressionSyntax startAtSyntax;

        public Rewriter(SemanticModel s, LiteralExpressionSyntax syn)
        {
            semanticModel = s;
            startAtSyntax = syn;
        }

        /// <summary>
        ///   Visits the given <see cref="ElementAccessExpressionSyntax"/>,
        ///   optionally replacing its index by 
        /// </summary>
        public override SyntaxNode VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            var arguments = node.ArgumentList.Arguments;

            for (int i = 0; i < arguments.Count; i++)
            {
                ArgumentSyntax index = node.ArgumentList.Arguments[i];

                // Ensure the index is an int
                IOperation operation = semanticModel.GetOperation(index.Expression);

                if (operation?.Type?.MetadataName != typeof(int).Name)
                    continue;


                // Replace expr[index] by expr[index - startAt]
                ArgumentSyntax newIndex = SyntaxFactory.Argument(
                    SyntaxFactory.BinaryExpression(SyntaxKind.SubtractExpression, index.Expression, startAtSyntax)
                );

                arguments = arguments.RemoveAt(i)
                                     .Insert(i, newIndex);
            }

            return node.WithArgumentList(node.ArgumentList.WithArguments(arguments));
        }
    }
}
