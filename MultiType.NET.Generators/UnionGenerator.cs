namespace MultiType.NET.Generators;

using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

/// <summary>
/// UnionGenerator is a source generator that generates union types in a strongly-typed manner.
/// It leverages attributes to infer and generate the associated types, contractors, implicit conversions,
/// and other relevant members required for union usage.
/// </summary>
/// <remarks>
/// This generator targets types annotated with the <c>GenerateUnionAttribute</c>. The attribute must
/// include the types to be encompassed by the union. Supported union types can include between 2 to 8
/// distinct types.
/// Generated union types are immutable and implement the necessary functionality to operate as
/// discriminated unions, allowing type-safe functionality with provided methods.
/// </remarks>
/// <example>
/// To use this generator:
/// 1. Annotate a struct or class with the <c>GenerateUnionAttribute</c>.
/// 2. Specify the desired types for the union in the attribute's constructor.
/// The generator will create the necessary implementation for a strongly-typed union.
/// </example>
/// <threadsafety>
/// The generated unions are thread-safe, given their immutable nature.
/// </threadsafety>
[Generator]
public class UnionGenerator : IIncrementalGenerator
{
    /// <summary>
    /// Initializes the source generator by registering syntax and source processing steps.
    /// </summary>
    /// <param name="context">
    /// The generator initialization context, used to register source outputs and syntax providers.
    /// </param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var typeDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => s is TypeDeclarationSyntax { AttributeLists.Count: > 0 },
                transform: static (ctx, _) =>
                {
                    var typeDecl = (TypeDeclarationSyntax)ctx.Node;
                    var symbol = ctx.SemanticModel.GetDeclaredSymbol(typeDecl) as INamedTypeSymbol;
                    return symbol;
                })
            .Where(symbol => symbol is not null)
            .Where(symbol => symbol!.GetAttributes().Any(attr =>
                attr.AttributeClass?.ToDisplayString() == "MultiType.NET.Generators.GenerateUnionAttribute"));

        context.RegisterSourceOutput(typeDeclarations, (ctx, symbol) =>
        {
            var attr = symbol?.GetAttributes().FirstOrDefault(a =>
                a.AttributeClass?.ToDisplayString() == "MultiType.NET.Generators.GenerateUnionAttribute");
            
            if (symbol is null || attr is null || attr.ConstructorArguments.Length == 0)
                return;

            var typeArgs = attr.ConstructorArguments[0].Values
                .Select(v => v.Value as INamedTypeSymbol)
                .Where(t => t != null)
                .ToList();

            if (typeArgs.Count is < 2 or > 8)
            {
                return; // Could emit diagnostic if desired
            }

            string ns = symbol.ContainingNamespace.IsGlobalNamespace
                ? "Generated"
                : symbol.ContainingNamespace.ToDisplayString();

            string unionName = symbol.Name;
            string typeParams = string.Join(", ",
                typeArgs.Select(t => t!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));
            string genericUnion = $"global::MultiType.NET.Core.Unions.Union<{typeParams}>";

            var sb = new StringBuilder();

            sb.AppendLine($"namespace {ns}");
            sb.AppendLine("{");
            sb.AppendLine($"    public readonly partial struct {unionName} : global::MultiType.NET.Core.IUnion");
            sb.AppendLine("    {");
            sb.AppendLine($"        private readonly {genericUnion} _inner;");
            sb.AppendLine();
            sb.AppendLine($"        public {unionName}({genericUnion} value) => _inner = value;");
            sb.AppendLine();

            // Add implicit conversion for each type
            foreach (var t in typeArgs)
                sb.AppendLine(
                    $"        public static implicit operator {unionName}({t}) => new({t?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)} value);");

            sb.AppendLine();
            sb.AppendLine("        public object? Value => _inner.Value;");
            sb.AppendLine("        public Type Type => _inner.Type;");
            sb.AppendLine("        public bool Is<T>() => _inner.Is<T>();");
            sb.AppendLine("        public T As<T>() => _inner.As<T>();");
            sb.AppendLine("        public override string ToString() => _inner.ToString() ?? string.Empty;");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            ctx.AddSource($"{unionName}_Generated.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        });
    }
}