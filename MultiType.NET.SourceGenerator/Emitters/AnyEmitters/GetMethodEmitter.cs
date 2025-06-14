namespace MultiType.NET.SourceGenerator.Emitters.AnyEmitters;

using System;
using System.Linq;
using System.Text;

internal static class GetMethodEmitter
{
    public static void EmitGetMethods(StringBuilder sb, int arity)
    {
        // Generate Get methods for each type parameter
        for (var i = 1; i <= arity; i++)
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

        var hasRemainder = arity > 2;
        GenerateGetTn(sb, arity, index, hasRemainder, remainderTypes);
        if (hasRemainder)
        {
            // generate GetTn without remainder overload
            GenerateGetTn(sb, arity, index, false, remainderTypes);
        }
    }

    private static void GenerateGetTn(StringBuilder sb, int arity, int index, bool hasRemainder,
        string remainderTypes)
    {
        var signature = hasRemainder
            ? $"public T{index} GetT{index}(out Any<{remainderTypes}> remainder)"
            : $"public T{index} GetT{index}()";

        sb.AppendLine($$"""
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        {{signature}}
                        {
                            {{(hasRemainder ? $$"""
                                                remainder = TypeIndex switch
                                                   {
                                                       {{
                                                           string.Join(", \n", Enumerable.Range(1, arity)
                                                               .Where(i => i != index)
                                                               .Select(i => $"{i} => Any<{remainderTypes}>.From((T{i})Value!)"))
                                                       }},
                                                       _ => default
                                                   };
                                                """ : "")}}

                            try
                            {
                                Guards.ThrowIfNotInitialized(TypeIndex, typeof(T{{index}}));
                                Guards.ThrowIfNotOutOfRange(TypeIndex, {{arity}});
                                Guards.ThrowIfNull(Value, typeof(T{{index}}), TypeIndex);
                                Guards.ThrowIfTypeMismatch<T{{index}}>(TypeIndex, {{index}}, Value, out T{{index}} typedValue);

                                return typedValue;
                            }
                            catch (Exception ex) when (ex is not InvalidAnyStateException)
                            {
                                // Wrap unexpected exceptions (Better Exception Handling)
                                throw new InvalidAnyStateException(
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
        for (var i = 1; i <= arity; i++)
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

        var hasRemainder = arity > 2;
        GenerateTryGetTn(sb, arity, index, hasRemainder, remainderTypes);
        if (hasRemainder)
        {
            // generate TryGetTn without remainder overload
            GenerateTryGetTn(sb, arity, index, false, remainderTypes);
        }
    }

    private static void GenerateTryGetTn(StringBuilder sb, int arity, int index, bool hasRemainder, string remainderTypes)
    {
        var signature = hasRemainder
            ? $"public bool TryGetT{index}(out T{index}? value, out Any<{remainderTypes}> remainder)"
            : $"public bool TryGetT{index}(out T{index}? value)";
        sb.AppendLine($$"""
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        {{signature}}
                        {
                            value = default;
                            {{(hasRemainder ? $"remainder = default;" : "")}}

                            if (TypeIndex == {{index}} && Value is T{{index}} t{{index}})
                            {
                               value = t{{index}};
                               return true;
                            }
                            
                            {{GenerateReminderLogicForTryGetTn(arity, index, remainderTypes, hasRemainder)}}
                            
                            return false;
                        }

                        """);
    }

    private static string GenerateReminderLogicForTryGetTn(int arity, int index, string remainderTypes,
        bool hasRemainder)
    {
        if (!hasRemainder)
        {
            return string.Empty;
        }

        return string.Join(" else ", Enumerable.Range(1, arity)
            .Where(i => i != index)
            .Select(i => $"if(TypeIndex == {i} && Value is T{i} t{i}) remainder = Any<{remainderTypes}>.From(t{i});"));
    }
}