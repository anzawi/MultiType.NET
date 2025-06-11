namespace MultiType.NET.SourceGenerator.Emitters.UnionEmitters;

using System.Linq;
using System.Text;

internal static class SwitchMethodEmitter
{
    public static void EmitSwitchMethods(StringBuilder sb, int arity)
    {
        EmitSyncSwitch(sb, arity);
        EmitAsyncSwitch(sb, arity);
        EmitSwitchOrDefault(sb, arity);
    }

    private static void EmitSyncSwitch(StringBuilder sb, int arity)
    {
        var parameters = string.Join(", ",
            Enumerable.Range(1, arity)
                .Select(i => $"Action<T{i}> case{i}"));

        var cases = string.Join("\n            ",
            Enumerable.Range(1, arity)
                .Select(i => $"case {i} when Value is T{i} v{i}: case{i}(v{i}); break;"));

        sb.AppendLine($$"""
            /// <summary>
            /// Executes the appropriate action based on the union type.
            /// </summary>
            public void Switch({{parameters}})
            {
                switch (TypeIndex)
                {
                    {{cases}}
                    default:
                     throw new InvalidUnionStateException("Union is not in a valid state for switching");
                };
            }

        """);
    }

    private static void EmitAsyncSwitch(StringBuilder sb, int arity)
    {
        var parameters = string.Join(", ",
            Enumerable.Range(1, arity)
                .Select(i => $"Func<T{i}, Task> case{i}"));

        var cases = string.Join("\n            ",
            Enumerable.Range(1, arity)
                .Select(i =>
                    $"case {i} when Value is T{i} v{i}: await case{i}(v{i}); break;"));

        sb.AppendLine($$"""
            /// <summary>
            /// Asynchronously executes the appropriate action based on the union type.
            /// </summary>
            public async Task SwitchAsync({{parameters}})
            {
                switch (TypeIndex)
                {
                    {{cases}}
                    default:
                        throw new InvalidUnionStateException("Union is not in a valid state for switching");
                }
            }

        """);
    }

    private static void EmitSwitchOrDefault(StringBuilder sb, int arity)
    {
        var parameters = string.Join(", ",
            Enumerable.Range(1, arity)
                .Select(i => $"Action<T{i}> case{i}"));

        sb.AppendLine($$"""
            /// <summary>
            /// Executes the appropriate action if the union is initialized; otherwise, runs default fallback.
            /// </summary>
            public void SwitchOrDefault({{parameters}}, Action fallback)
            {
                switch (TypeIndex)
                {
        """);

        foreach (var i in Enumerable.Range(1, arity))
        {
            sb.AppendLine($"""
                    case {i} when Value is T{i} v{i}: case{i}(v{i}); break;
            """);
        }

        sb.AppendLine("""
                    default:
                        fallback();
                        break;
                }
            }
        """);
    }
}
