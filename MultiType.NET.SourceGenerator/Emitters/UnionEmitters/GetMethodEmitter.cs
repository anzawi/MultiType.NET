namespace MultiType.NET.SourceGenerator.Emitters.UnionEmitters;

using System.Linq;
using System.Text;

internal static class GetMethodEmitter
{
    public static void EmitGetMethods(StringBuilder sb, int arity)
    {
        // Generate Get methods for each type parameter
        for (int i = 1; i <= arity; i++)
        {
            EmitGetMethod(sb, arity, i);
        }
    }

    private static void EmitGetMethod(StringBuilder sb, int arity, int index)
    {
        // Create remainder type string (all types except current index)
        var remainderTypes = string.Join(", ",
            Enumerable.Range(1, arity)
                .Where(i => i != index)
                .Select(i => $"T{i}"));

        sb.AppendLine($$"""
                        public T{{index}} GetT{{index}}(out Union<{{remainderTypes}}>? remainder)
                        {
                            // Initialize out parameter early (Defensive Programming)
                            remainder = default;

                            try
                            {
                                Guards.ThrowIfNotInitialized(TypeIndex, typeof(T{{index}}));
                                Guards.ThrowIfNotOutOfRange(TypeIndex, {{arity}});
                                Guards.ThrowIfNull(Value, typeof(T{{index}}), TypeIndex);
                                Guards.ThrowIfTypeMismatch<T{{index}}>(TypeIndex, {{index}}, Value, out T{{index}} typedValue);

                                return typedValue;
                            }
                            catch (Exception ex) when (ex is not InvalidUnionStateException)
                            {
                                // Wrap unexpected exceptions (Better Exception Handling)
                                throw new InvalidUnionStateException(
                                    $"Unexpected error getting value of type {typeof(T{{index}}).Name}\nTypeIndex: {TypeIndex}\nValue type: {Value?.GetType().Name ?? "null"}", 
                                    ex);
                            }
                        }

                        """);

}
}

internal static class TryGetMethodEmitter
{
    public static void EmitTryGetMethods(StringBuilder sb, int arity)
    {
        // Generate TryGet methods for each type parameter
        for (int i = 1; i <= arity; i++)
        {
            EmitTryGetMethod(sb, arity, i);
        }
    }

    private static void EmitTryGetMethod(StringBuilder sb, int arity, int index)
    {
        // Create remainder type string (all types except current index)
        var remainderTypes = string.Join(", ",
            Enumerable.Range(1, arity)
                .Where(i => i != index)
                .Select(i => $"T{i}"));

        sb.AppendLine($$"""
                        public bool TryGetT{{index}}(out T{{index}}? value, out Union<{{remainderTypes}}>? remainder)
                        {
                            value = default;
                            remainder = default;

                            try
                            {
                                if (TypeIndex == 0 || TypeIndex > {{arity}}) return false;
                                if (TypeIndex != {{index}}) return false;
                                if (Value is null) return false;

                                if (Value is T{{index}} typedValue)
                                {
                                    value = typedValue;
                                    return true;
                                }

                                return false;
                            }
                            catch
                            {
                                return false;
                            }
                        }

                        """);
    }
}