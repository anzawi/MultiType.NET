using System.Security.Cryptography;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using MultiType.NET.Generators.Constants;

namespace MultiType.NET.Generators.Helpers;

public static class Functions
{
    public static int CountTypesInNamespace(Compilation compilation, string @namespace)
    {
        var parts = @namespace.Split('.');
        var current = compilation.GlobalNamespace;

        foreach (var part in parts)
        {
            current = current?.GetNamespaceMembers().FirstOrDefault(ns => ns.Name == part);
            if (current == null)
                return 0;
        }


        return CountTypesRecursive(current!);
    }
    
    public static void ReportNewDiagnostic(SourceProductionContext ctx, INamedTypeSymbol symbol, DiagnosticDescriptor descriptor)
    {
        ctx.ReportDiagnostic(Diagnostic.Create(descriptor,
            symbol.Locations.FirstOrDefault() ?? Location.None));

        if (descriptor.DefaultSeverity == DiagnosticSeverity.Error)
        {
            GenerateFile(ctx, $"{symbol.Name}{Consts.AnyTypeGeneratedFileSuffix}",
                @$"
                #error {descriptor.Id} - [{descriptor.Title}] - {descriptor.MessageFormat}
                ");
        }
    }
    
    public static void ReportNewDiagnostic(SourceProductionContext ctx, INamedTypeSymbol symbol, DiagnosticDescriptor descriptor, params object[] args)
    {
        var updatedDescriptor = new DiagnosticDescriptor(
            id: descriptor.Id,
            title: "Invalid GenerateAny usage",
            messageFormat: descriptor.MessageFormat + args.Aggregate("", (current, arg) => current + "{" + arg + "}"),
            category: Consts.DiagnosticCategory,
            descriptor.DefaultSeverity,
            isEnabledByDefault: true);
        ReportNewDiagnostic(ctx, symbol, updatedDescriptor);
    }

    
    public static IncrementalValueProvider<bool> IsEmitGeneratedFileEnabled(IncrementalGeneratorInitializationContext context)
    {
        var emitGeneratedFiles = context.AnalyzerConfigOptionsProvider
            .Select((provider, _) =>
            {
                provider.GlobalOptions.TryGetValue("build_property.EmitCompilerGeneratedFiles", out var value);
                return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
            });
        return emitGeneratedFiles;
    }

    public static void GenerateFile(SourceProductionContext ctx, string filename, string content)
    {
        ctx.AddSource(filename, SourceText.From(content, Encoding.UTF8));
    }

    private static int CountTypesRecursive(INamespaceSymbol @namespace)
    {
        return @namespace.GetTypeMembers().Length +
               @namespace.GetNamespaceMembers().Sum(CountTypesRecursive);
    }
}