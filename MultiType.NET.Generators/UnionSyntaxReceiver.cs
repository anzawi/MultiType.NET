namespace MultiType.NET.Generators;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/// <summary>
/// Collects type declarations that are marked as partial and have attributes applied to them.
/// The <see cref="UnionSyntaxReceiver"/> class is used during source generation to gather relevant syntax nodes for processing.
/// </summary>
public class UnionSyntaxReceiver : ISyntaxReceiver
{
    /// <summary>
    /// Gets the collection of <see cref="TypeDeclarationSyntax"/> instances
    /// that meet specific criteria defined in the <see cref="UnionSyntaxReceiver"/> logic.
    /// These include having attributes, being declared as partial, and being valid type declarations.
    /// </summary>
    public List<TypeDeclarationSyntax> Candidates { get; } = new();

    /// <summary>
    /// Invoked when a syntax node is visited to determine if it matches certain criteria.
    /// Adds the node to the list of candidates if it meets the conditions.
    /// </summary>
    /// <param name="node">The syntax node being visited.</param>
    public void OnVisitSyntaxNode(SyntaxNode node)
    {
        if (node is TypeDeclarationSyntax tds &&
            tds.AttributeLists.Count > 0 &&
            tds.Modifiers.Any(m => m.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword)))
        {
            this.Candidates.Add(tds);
        }
    }
}