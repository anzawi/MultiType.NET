using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MultiType.NET.Generators.Helpers;

internal static class CodeFormatter
{
    public static string Format(string sourceCode)
    {
        var tree = CSharpSyntaxTree.ParseText(sourceCode);
        var root = tree.GetRoot();
        var formatted = root.NormalizeWhitespace();
        return formatted.ToFullString();
    }
}