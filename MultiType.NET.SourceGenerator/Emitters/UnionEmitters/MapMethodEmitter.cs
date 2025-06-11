namespace MultiType.NET.SourceGenerator.Emitters.UnionEmitters;

using System.Linq;
using System.Text;

internal static class MapMethodEmitter
{
    public static void EmitMapMethods(StringBuilder sb, int arity)
    {
        // Basic Map to common result
        EmitBasicMap(sb, arity);

        // Map to new Union
        EmitUnionMap(sb, arity);

        // Map with value/reference type distinction
        EmitValueTypeMap(sb, arity);

        // Async Map
        EmitAsyncMap(sb, arity);

        // Map with null handling
        EmitMapOrDefault(sb, arity);

        // Map with exception handling
        EmitMapSafe(sb, arity);

        // Conditional Map
        EmitMapWhere(sb, arity);

        // Context-aware Map
        EmitMapWithContext(sb, arity);
    }

    private static void EmitBasicMap(StringBuilder sb, int arity)
    {
        var parameters = string.Join(", ",
            Enumerable.Range(1, arity)
                .Select(i => $"Func<T{i}, TResult> map{i}"));

        var cases = string.Join("\n            ",
            Enumerable.Range(1, arity)
                .Select(i => $"{i} when Value is T{i} v{i} => map{i}(v{i}),"));

        sb.AppendLine($$"""
                        /// <summary>
                        /// Maps the value contained in this union to a new type using a common result type.
                        /// </summary>
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public TResult Map<TResult>({{parameters}})
                        {
                            return TypeIndex switch
                            {
                                {{cases}}
                                _ => throw new InvalidUnionStateException("Union is not in a valid state for mapping")
                            };
                        }

                        """);
    }

    private static void EmitUnionMap(StringBuilder sb, int arity)
    {
        var typeParams = string.Join(", ",
            Enumerable.Range(1, arity).Select(i => $"TResult{i}"));

        var parameters = string.Join(", ",
            Enumerable.Range(1, arity)
                .Select(i => $"Func<T{i}, TResult{i}> map{i}"));

        var cases = string.Join("\n            ",
            Enumerable.Range(1, arity)
                .Select(i => $"{i} when Value is T{i} v{i} => map{i}(v{i}),"));

        sb.AppendLine($$"""
                        /// <summary>
                        /// Maps the value to a new union type with different type parameters.
                        /// </summary>
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public Union<{{typeParams}}> MapUnion<{{typeParams}}>({{parameters}})
                        {
                            return TypeIndex switch
                            {
                                {{cases}}
                                _ => throw new InvalidUnionStateException("Union is not in a valid state for mapping")
                            };
                        }

                        """);
    }

    private static void EmitValueTypeMap(StringBuilder sb, int arity)
    {
        sb.AppendLine($$"""
                        /// <summary>
                        /// Maps the value using different strategies for value types and reference types.
                        /// </summary>
                        public TResult MapValue<TResult>(
                            Func<ValueType, TResult> valueTypeMapper,
                            Func<object, TResult> referenceTypeMapper)
                        {
                            if (Value is null)
                                throw new InvalidUnionStateException("Cannot map null value");

                            return _isValueType
                                ? valueTypeMapper(_valueType!)
                                : referenceTypeMapper(_referenceType!);
                        }

                        """);
    }

    private static void EmitAsyncMap(StringBuilder sb, int arity)
    {
        var parameters = string.Join(", ",
            Enumerable.Range(1, arity)
                .Select(i => $"Func<T{i}, Task<TResult>> map{i}"));

        var cases = string.Join("\n            ",
            Enumerable.Range(1, arity)
                .Select(i => $"{i} when Value is T{i} v{i} => await map{i}(v{i}),"));

        sb.AppendLine($$"""
                        /// <summary>
                        /// Asynchronously maps the union value.
                        /// </summary>
                        public async Task<TResult> MapAsync<TResult>({{parameters}})
                        {
                            return TypeIndex switch
                            {
                                {{cases}}
                                _ => throw new InvalidUnionStateException("Union is not in a valid state for mapping")
                            };
                        }

                        """);
    }

    private static void EmitMapOrDefault(StringBuilder sb, int arity)
    {
        var parameters = string.Join(", ",
            Enumerable.Range(1, arity)
                .Select(i => $"Func<T{i}, TResult> map{i}"));

        var cases = string.Join("\n            ",
            Enumerable.Range(1, arity)
                .Select(i => $"{i} when Value is T{i} v{i} => map{i}(v{i}),"));

        sb.AppendLine($$"""
                        /// <summary>
                        /// Maps the value with null-safety, returning a default value if null.
                        /// </summary>
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public TResult MapOrDefault<TResult>(
                            {{parameters}},
                            TResult defaultValue = default!)
                        {
                            if (IsNull) return defaultValue;

                            return TypeIndex switch
                            {
                                {{cases}}
                                _ => defaultValue
                            };
                        }

                        """);
    }

    private static void EmitMapSafe(StringBuilder sb, int arity)
    {
        var parameters = string.Join(", ",
            Enumerable.Range(1, arity)
                .Select(i => $"Func<T{i}, TResult> map{i}"));

        var cases = string.Join("\n            ",
            Enumerable.Range(1, arity)
                .Select(i => $"{i} when Value is T{i} v{i} => map{i}(v{i}),"));

        sb.AppendLine($$"""
                        /// <summary>
                        /// Maps the value with exception handling.
                        /// </summary>
                        public TResult MapSafe<TResult>(
                            {{parameters}},
                            Func<Exception, TResult> errorHandler)
                        {
                            try
                            {
                                return TypeIndex switch
                                {
                                    {{cases}}
                                    _ => throw new InvalidUnionStateException("Union is not in a valid state for mapping")
                                };
                            }
                            catch (Exception ex)
                            {
                                return errorHandler(ex);
                            }
                        }

                        """);
    }

    private static void EmitMapWhere(StringBuilder sb, int arity)
    {
        var parameters = string.Join(", ",
            Enumerable.Range(1, arity)
                .SelectMany(i => new[]
                {
                    $"Func<T{i}, bool> predicate{i}",
                    $"Func<T{i}, TResult> map{i}"
                }));

        var cases = string.Join("\n            ",
            Enumerable.Range(1, arity)
                .Select(i => $"{i} when Value is T{i} v{i} && predicate{i}(v{i}) => map{i}(v{i}),"));

        sb.AppendLine($$"""
                        /// <summary>
                        /// Maps the value conditionally based on predicates.
                        /// </summary>
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public TResult MapWhere<TResult>(
                            {{parameters}},
                            TResult defaultValue = default!)
                        {
                            return TypeIndex switch
                            {
                                {{cases}}
                                _ => defaultValue
                            };
                        }

                        """);
    }

    private static void EmitMapWithContext(StringBuilder sb, int arity)
    {
        var typeParams = string.Join(", ",
            Enumerable.Range(1, arity).Select(i => $"T{i}"));

        var parameters = string.Join(", ",
            Enumerable.Range(1, arity)
                .Select(i => $"Func<Union<{typeParams}>, T{i}, TResult> map{i}"));

        var cases = string.Join("\n            ",
            Enumerable.Range(1, arity)
                .Select(i => $"{i} when Value is T{i} v{i} => map{i}(this, v{i}),"));

        sb.AppendLine($$"""
                        /// <summary>
                        /// Maps with access to the entire union instance.
                        /// </summary>
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public TResult MapWithContext<TResult>({{parameters}})
                        {
                            return TypeIndex switch
                            {
                                {{cases}}
                                _ => throw new InvalidUnionStateException("Union is not in a valid state for mapping")
                            };
                        }

                        """);
    }
}