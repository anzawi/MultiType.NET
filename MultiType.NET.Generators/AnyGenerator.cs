using MultiType.NET.Generators.Constants;
using MultiType.NET.Generators.Helpers;

namespace MultiType.NET.Generators;

using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

/// <summary>
/// AnyGenerator is a source generator that generates any types in a strongly-typed manner.
/// It leverages attributes to infer and generate the associated types, contractors, implicit conversions,
/// and other relevant members required for any usage.
/// </summary>
/// <remarks>
/// This generator targets types annotated with the <c>GenerateAnyAttribute</c>. The attribute must
/// include the types to be encompassed by the any. Supported any types can include between 2 to n
/// distinct types.
/// Generated any types are immutable and implement the necessary functionality to operate as
/// discriminated anys, allowing type-safe functionality with provided methods.
/// </remarks>
/// <example>
/// To use this generator:
/// 1. Annotate a struct or class with the <c>GenerateAnyAttribute</c>.
/// 2. Specify the desired types for the any in the attribute's constructor.
/// The generator will create the necessary implementation for a strongly-typed any.
/// </example>
/// <threadsafety>
/// The generated anys are thread-safe, given their immutable nature.
/// </threadsafety>
[Generator]
public class AnyGenerator : IIncrementalGenerator
{
    /// <summary>
    /// Initializes the source generator by registering syntax and source processing steps.
    /// </summary>
    /// <param name="context">
    /// The generator initialization context, used to register source outputs and syntax providers.
    /// </param>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var emitGeneratedFiles = Functions.IsEmitGeneratedFileEnabled(context);
        
        
        IncrementalValuesProvider<((INamedTypeSymbol?, Compilation), bool)> ctx =
            GenerateContext(context, emitGeneratedFiles)!;

        context.RegisterSourceOutput(ctx,
            static (ctx, tuple) =>
            {
                var ((symbol, compilation), emitEnabled) = tuple;
                
                if (!IsCompatibleSdkRuntime(ctx, symbol, compilation, emitEnabled)) return;

                var attr = symbol?.GetAttributes().FirstOrDefault(a =>
                    a.AttributeClass?.ToDisplayString() == $"{Consts.AttributeNamespace}.{Consts.AttributeName}");

                if (symbol is null || attr is null || attr.ConstructorArguments.Length == 0)
                    return;
               
                var isTopLevel = symbol.ContainingType is null;

                if (!isTopLevel)
                {
                    Functions.ReportNewDiagnostic(ctx, symbol, Notifications.TopLevelError);
                    return;
                }

                var typeArgs = attr.ConstructorArguments[0].Values
                    .Select(v => v.Value as INamedTypeSymbol)
                    .Where(t => t != null)
                    .ToList();


                var ns = symbol.ContainingNamespace.IsGlobalNamespace
                    ? "Generated"
                    : $"{symbol.ContainingNamespace.ToDisplayString()}";

                var anyName = symbol.Name;
                
                var typeParams = string.Join(", ",
                    typeArgs.Select(t => t!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)));
                var genericAny = $"{Consts.AnysCoreNamespace}.Any<{typeParams}>";

                if (typeArgs.Count < 2)
                {
                    Functions.ReportNewDiagnostic(ctx, symbol, Notifications.ObjectIsSingleTypeError);
                    return;
                }

                if (Functions.CountTypesInNamespace(compilation, $"{Consts.AnysCoreNamespace}") > typeArgs.Count)
                {
                    Functions.ReportNewDiagnostic(ctx, symbol, Notifications.ObjectIsotOfTypesError, typeArgs.Count);
                    return;
                }

                var accessibility = symbol.DeclaredAccessibility;
                var sb = new StringBuilder();

                sb.AppendLine($$"""
                                {{StructHeader(ns, anyName, typeParams, genericAny, accessibility)}}

                                {{GenerateImplicits(typeArgs, anyName, genericAny)}}
                                {{GenerateProperties(genericAny)}}
                                {{GenerateMethods(genericAny, anyName)}}
                                    }
                                    {{GenerateJsonConvertorClass(anyName, genericAny, accessibility)}}
                                """);

                var formattedContent = CodeFormatter.Format($$"""
                                                              {{Consts.GeneratedCodeHeader}}

                                                              #nullable enable

                                                              {{sb}}
                                                              """);

                Functions.GenerateFile(ctx, $"{symbol.Name}{Consts.AnyTypeGeneratedFileSuffix}", formattedContent);
            });
    }


    private static IncrementalValuesProvider<((INamedTypeSymbol Symbol, Compilation Compilation), bool)>
        GenerateContext(
            IncrementalGeneratorInitializationContext context,
            IncrementalValueProvider<bool> emitGeneratedFiles)
    {
        var typeDeclarations = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: $"{Consts.AttributeNamespace}.{Consts.AttributeName}",
                predicate: static (syntaxNode, _) => syntaxNode is TypeDeclarationSyntax,
                transform: static (context, _) =>
                {
                    var symbol = (INamedTypeSymbol?)context.TargetSymbol;
                    return symbol;
                })
            .Where(static symbol => symbol is not null)!;

            var ctx = typeDeclarations
            .Combine(context.CompilationProvider)
            .Combine(emitGeneratedFiles)
            .Select<((INamedTypeSymbol?, Compilation), bool), ((INamedTypeSymbol, Compilation), bool)>(
                static (data, _) =>
                {
                    var ((symbol, compilation), emit) = data;
                    return ((symbol!, compilation), emit);
                });

        return ctx;
    }



    private static string StructHeader(string ns, string anyName, string typeParams, string genericAny, Accessibility accessibility)
    {
        return $$"""
                 #pragma warning disable 1591
                 namespace {{ns}};
                 using global::System.Runtime.CompilerServices;
                 using global::System.Text.Json.Serialization;
                 using global::MultiType.NET.Core.Serialization.Generated;
                 using global::System.Text.Json;
                 [JsonConverter(typeof({{anyName}}JsonConverter))]
                 {{(accessibility == Accessibility.Internal ? "internal" : "public")}} readonly partial struct {{anyName}} : global::MultiType.NET.Core.IAny
                 {
                 private readonly {{genericAny}} _inner;

                 public {{anyName}}({{genericAny}} value) => _inner = value;
                 
                 public static bool TryParse(string input, IFormatProvider? _, out global::{{ns}}.{{anyName}} result) 
                 {
                    var success = {{genericAny}}.TryParse(input, _, out var inner);
                    result = new {{ns}}.{{anyName}}(inner);
                    return success;
                 }
                 """;
    }

    private static string GenerateImplicits(List<INamedTypeSymbol?> typeArgs, string anyName, string genericAny)
    {
        return string.Join("\n",
            typeArgs.Select(t =>
                $"public static implicit operator {anyName}({t} value) => new({genericAny}.From(value));"));
    }

    private static string GenerateProperties(string genericAny)
    {
        return $"""
                public byte TypeIndex => _inner.TypeIndex;
                /// <inheritdoc/>
                public object? Value => _inner.Value;
                /// <inheritdoc/>
                public Type Type => _inner.Type;
                /// <inheritdoc/>
                public bool HasValue => _inner.HasValue;
                /// <inheritdoc/>
                public bool IsNull => _inner.IsNull;
                /// <summary>
                /// The set of allowed types for this Any.
                /// </summary>
                public Type[] AllowedTypes => {genericAny}.AllowedTypes;
                /// <summary>
                /// The type of the value contained in this Any.
                /// </summary>
                public {genericAny} Inner => _inner;
                """;
    }

    private static string GenerateMethods(string genericAny, string anyName)
    {
        return $$"""
                 /// <inheritdoc/>
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public bool Is<T>() => _inner.Is<T>();
                 /// <inheritdoc/>
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public T As<T>() => _inner.As<T>();

                 /// <inheritdoc/>
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public T? AsNullable<T>()
                     where T : class => _inner.AsNullable<T>();
                 /// <inheritdoc/>
                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public T? AsNullableStruct<T>()
                     where T : struct => _inner.AsNullableStruct<T>();

                 [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static {{anyName}} From(object? value) => new {{anyName}}({{genericAny}}.From(value));
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                 public static bool TryFrom(object? value, out {{anyName}} result)
                 {
                    var success = {{genericAny}}.TryFrom(value, out var inner);
                    result = new {{anyName}}(inner);
                    return success;
                 }
                 """;
    }

    private static string GenerateJsonConvertorClass(string anyName,  string genericAny, Accessibility accessibility)
    {
        return $$"""
                 {{(accessibility == Accessibility.Internal ? "internal" : "public")}} sealed class {{anyName}}JsonConverter : JsonConverter<{{anyName}}>
                 {
                     private static readonly JsonConverter<{{genericAny}}> _innerConverter =
                         (JsonConverter<{{genericAny}}>) 
                             (JsonSerializerOptions.Default.GetConverter(typeof({{genericAny}})) 
                             ?? throw new InvalidOperationException("Missing AnyJsonConverter."));
                 
                     public override {{anyName}} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
                     {
                         // Use the configured converter (honors custom options)
                         var inner = JsonSerializer.Deserialize<{{genericAny}}>(ref reader, options);
                         return new {{anyName}}(inner!);
                     }
                 
                     public override void Write(Utf8JsonWriter writer, {{anyName}} value, JsonSerializerOptions options)
                     {
                         // Avoid allocating unless needed
                         JsonSerializer.Serialize(writer, value.Inner, options);
                     }
                 }
                 """;
    }
    
    private static bool IsCompatibleSdkRuntime(
        SourceProductionContext ctx,
        INamedTypeSymbol? symbol,
        Compilation compilation, bool emitEnabled)
    {
        var systemRuntimeVersion = compilation
            .ReferencedAssemblyNames
            .FirstOrDefault(r => r.Name == "System.Runtime")?.Version;

        if (!emitEnabled)
        {
            Functions.ReportNewDiagnostic(ctx, symbol!, Notifications.EmitGeneratedFileDisabledError);
            return false;
        }

        if (systemRuntimeVersion?.Major < 8)
        {
            Functions.ReportNewDiagnostic(ctx, symbol!, Notifications.TargetUnsupportedFramework, systemRuntimeVersion.Major);
        }
        
        if (systemRuntimeVersion?.Major == 8)
        {
            Functions.ReportNewDiagnostic(ctx, symbol!, Notifications.TargetNet8Warning);
        }

        return true;
    }
}