using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MultiType.NET.Generators;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class ObsoleteAttributeSuppressor : DiagnosticSuppressor
{
    private static readonly SuppressionDescriptor SuppressObsolete =
        new SuppressionDescriptor(
            id: "SUPP1001",
            suppressedDiagnosticId: "CS0618",
            justification: "Suppress obsolete warning for GenerateAnyAttribute when generator is active.");

    public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions =>
        ImmutableArray.Create(SuppressObsolete);

    public override void ReportSuppressions(SuppressionAnalysisContext context)
    {
        foreach (var diagnostic in context.ReportedDiagnostics)
        {
            if (diagnostic.Id != "CS0618")
                continue;

            var location = diagnostic.Location;
            var tree = location.SourceTree;
            if (tree is null)
                continue;

            var root = tree.GetRoot(context.CancellationToken);
            var node = root.FindNode(location.SourceSpan);

            // 🧠 Check if the diagnostic is on an AttributeSyntax node
            if (node is AttributeSyntax attributeSyntax)
            {
                var model = context.GetSemanticModel(tree);
                var type = model.GetTypeInfo(attributeSyntax, context.CancellationToken).Type;

                if (type?.ToDisplayString() == "MultiType.NET.Core.Attributes.GenerateAnyAttribute")
                {
                    context.ReportSuppression(Suppression.Create(SuppressObsolete, diagnostic));
                }
            }
        }
    }
}