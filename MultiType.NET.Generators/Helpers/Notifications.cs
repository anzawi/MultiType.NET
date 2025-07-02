using Microsoft.CodeAnalysis;
using MultiType.NET.Generators.Constants;

namespace MultiType.NET.Generators.Helpers;

internal abstract class Notifications
{
    // Error codes end with 9 -> 0009
    public static readonly DiagnosticDescriptor ObjectIsSingleTypeError = new(
        id: "ANYGEN1009",
        title: "Invalid GenerateAny usage",
        messageFormat: "The type MUST have at least 2 types to be encompassed by the any",
        category: Consts.DiagnosticCategory,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor ObjectIsotOfTypesError = new(
        id: "ANYGEN2009",
        title: "Invalid GenerateAny usage",
        messageFormat:
        "Any with arguments = '{0}' is not supported by default! the supported by default is 16 only, to make support +{0} you have to use MuliType.NET.SourceGenerator",
        category: Consts.DiagnosticCategory,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static readonly DiagnosticDescriptor EmitGeneratedFileDisabledError = new(
        id: "ANYGEN3009",
        title: "MultiType.NET Source Generator detected a misconfiguration",
        messageFormat: @"To enable file output for source generators, ensure the following MSBuild properties are set:
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
  <GeneratedFolder>Generated</GeneratedFolder>
  <CompilerGeneratedFilesOutputPath>$(GeneratedFolder)/$(TargetFramework)</CompilerGeneratedFilesOutputPath>",
        category: Consts.DiagnosticCategory,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    ); 
    
    public static readonly DiagnosticDescriptor TargetUnsupportedFramework = new(
        id: "ANYGEN4009",   
        title: "Improper Configuration",
        messageFormat: "MultiType.NET Source Generator detected unsupported target framework '{0}', Minimum supported target framework is 'net8.0'",
        category: Consts.DiagnosticCategory,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    ); 
    
    public static readonly DiagnosticDescriptor TopLevelError = new(
        id: "ANYGEN5009",
        title: "Class must be top level",
        messageFormat: $"Structs using {Consts.AttributeName.Replace("Attribute", "")} must be top level",
        category: Consts.DiagnosticCategory,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    ); 
    

    // Warning codes end with 6 -> 0006
    public static readonly DiagnosticDescriptor TargetNet8Warning = new(
        id: "ANYGEN1006",
        title: "Detected Build with .NET SDK 8.x.x (Roslyn Bug)",
        messageFormat: """
                       MultiType.NET `Anys` Generator detected that this project is being **compiled using .NET SDK 8.x.x**.

                       ⚠️ This SDK version includes a known bug in Roslyn that prevents generated files from being written to disk.
                       This affects the functionality of source generators like MultiType.NET.

                       🐞 Bug Reference:
                       Issue #70947 – “Can’t emit source generated code files”
                       https://github.com/dotnet/roslyn/issues/70947

                       ✅ To resolve:
                       - You may still target `net8.0`, but **must build the project using a different SDK**, such as:
                         - .NET SDK 9.x or higher
                         - .NET SDK 6.x or 7.x

                       💡 To override SDK used in builds, create a `global.json` file with:
                         {
                           "sdk": {
                             "version": "9.0.0",
                             "rollForward": "latestMinor"
                           }
                         }

                       ❌ Building with SDK 8.x.x will cause generated types to disappear, resulting in compile-time errors.
                       """,
        category: Consts.DiagnosticCategory,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    // Info codes end with 4 -> 0004
}