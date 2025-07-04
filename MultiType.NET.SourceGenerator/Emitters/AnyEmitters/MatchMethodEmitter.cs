namespace MultiType.NET.SourceGenerator.Emitters.AnyEmitters;

using System.Linq;
using System.Text;

internal static class MatchMethodEmitter
{
    public static void EmitMatchMethods(StringBuilder sb, int arity)
    {
        // Generate Match methods
        EmitMatchFuncMethod(sb, arity);
        EmitMatchActionMethod(sb, arity);
    }

    private static void EmitMatchFuncMethod(StringBuilder sb, int arity)
    {
        var caseParams = string.Join(", ", Enumerable.Range(1, arity)
            .Select(i => $"Func<T{i}, TResult> case{i}"));
        var cases = string.Join(",\n                    ",
            Enumerable.Range(1, arity).Select(i => $"{i} => case{i}((T{i})Value!)"));

        sb.AppendLine($$"""
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public TResult Match<TResult>({{caseParams}})
                        {
                            return this.TypeIndex switch
                            {
                                {{cases}},
                                _ => throw new InvalidOperationException("Any is not initialized."),
                            };
                        }

                        """);
    }

    private static void EmitMatchActionMethod(StringBuilder sb, int arity)
    {
        var caseParams = string.Join(", ", Enumerable.Range(1, arity)
            .Select(i => $"[MaybeNull] Action<T{i}> case{i}"));
        var cases = string.Join("\n                    ",
            Enumerable.Range(1, arity)
                .Select(i => $"case {i}: case{i}((T{i})Value!); break;"));

        sb.AppendLine($$"""
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public void Match({{caseParams}})
                        {
                            switch (this.TypeIndex)
                            {
                                {{cases}}
                                default: throw new InvalidOperationException("Any is not initialized.");
                            }
                        }

                        """);
    }
}

internal static class TryMatchMethodEmitter
{
    public static void EmitTryMatchMethods(StringBuilder sb, int arity)
    {
        EmitTryMatchFuncMethod(sb, arity);
        EmitTryMatchActionMethod(sb, arity);
    }

    private static void EmitTryMatchFuncMethod(StringBuilder sb, int arity)
    {
        var caseParams = string.Join(", ", Enumerable.Range(1, arity)
            .Select(i => $"Func<T{i}, TResult>? case{i} = null"));
        var cases = string.Join(",\n                    ",
            Enumerable.Range(1, arity).Select(i =>
                $"{i} when case{i} != null => case{i}((T{i})Value!)"));

        sb.AppendLine($$"""
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public TResult? TryMatch<TResult>({{caseParams}})
                        {
                            return this.TypeIndex switch
                            {
                                {{cases}},
                                _ => default,
                            };
                        }

                        """);
    }

    private static void EmitTryMatchActionMethod(StringBuilder sb, int arity)
    {
        var caseParams = string.Join(", ", Enumerable.Range(1, arity)
            .Select(i => $"[MaybeNull] Action<T{i}>? case{i} = null"));
        var cases = string.Join("\n                    ",
            Enumerable.Range(1, arity).Select(i =>
                $"case {i} when case{i} != null:\ncase{i}((T{i})Value!);\n                        break;"));

        sb.AppendLine($$"""
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public void TryMatch({{caseParams}})
                        {
                            switch (this.TypeIndex)
                            {
                                {{cases}}
                            }
                        }

                        """);
    }
}