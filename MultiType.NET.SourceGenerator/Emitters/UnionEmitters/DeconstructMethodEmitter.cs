namespace MultiType.NET.SourceGenerator.Emitters.UnionEmitters;

using System.Linq;
using System.Text;

internal static class DeconstructMethodEmitter
{
    public static void EmitDeconstructMethod(StringBuilder sb, int arity)
    {
        // Method signature
        var outParams = string.Join(", ",
            Enumerable.Range(1, arity).Select(i => $"out T{i} value{i}"));

        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Deconstructs the union into individual out variables based on the active type.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public void Deconstruct({outParams})");
        sb.AppendLine("{");

        // Initialize all values to default
        foreach (int i in Enumerable.Range(1, arity))
        {
            sb.AppendLine($"    value{i} = default!;");
        }

        sb.AppendLine();
        sb.AppendLine("    switch (TypeIndex)");
        sb.AppendLine("    {");

        foreach (int i in Enumerable.Range(1, arity))
        {
            sb.AppendLine($"        case {i} when Value is T{i} v{i}:");
            sb.AppendLine($"            value{i} = v{i};");
            sb.AppendLine("            break;");
            sb.AppendLine();
        }

        sb.AppendLine("        default:");
        sb.AppendLine("            throw new InvalidUnionStateException($\"\"\"");
        sb.AppendLine("                Unable to deconstruct union — invalid TypeIndex ({TypeIndex}) or type mismatch.");
        sb.AppendLine("                Actual value: {Value?.GetType().Name ?? \"null\"}");
        sb.AppendLine("            \"\"\");");

        sb.AppendLine("    }");
        sb.AppendLine("}");
        sb.AppendLine();
    }
}