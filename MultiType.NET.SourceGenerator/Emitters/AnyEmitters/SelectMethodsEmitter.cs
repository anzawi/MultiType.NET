namespace MultiType.NET.SourceGenerator.Emitters.AnyEmitters;

using System.Linq;
using System.Text;

public class SelectMethodsEmitter
{
    public static void EmitSelectMethods(StringBuilder sb, int arity)
{
    // Select
    var selectors = string.Join(", ",
        Enumerable.Range(1, arity).Select(i => $"Func<T{i}, TResult> selector{i}"));

    var selectCases = string.Join(",\n            ",
        Enumerable.Range(1, arity).Select(i => $"{i} when Value is T{i} v{i} => selector{i}(v{i})"));

    sb.AppendLine($$"""
        /// <summary>
        /// Projects the Any value into a new form using the specified selector functions.
        /// </summary>
        public TResult Select<TResult>({{selectors}})
        {
            return TypeIndex switch
            {
                {{selectCases}},
                _ => throw new InvalidAnyStateException("Any is not in a valid state for selection")
            };
        }
    """);

    // SelectOrDefault
    sb.AppendLine($$"""
        /// <summary>
        /// Projects the Any value using selector functions, or returns a fallback default.
        /// </summary>
        public TResult SelectOrDefault<TResult>({{selectors}}, TResult defaultValue = default!)
        {
            try { return Select({{string.Join(", ", Enumerable.Range(1, arity).Select(i => $"selector{i}"))}}); }
            catch { return defaultValue; }
        }
    """);

    // TrySelect
    sb.AppendLine($$"""
        /// <summary>
        /// Attempts to project the Any value using selector functions.
        /// Returns null if uninitialized or type mismatch.
        /// </summary>
        public TResult? TrySelect<TResult>({{selectors}})
        {
            if (TypeIndex == 0 || Value is null) return default;
            return Select({{string.Join(", ", Enumerable.Range(1, arity).Select(i => $"selector{i}"))}});
        }
    """);

    // SelectWithContext
    var contextSelectors = string.Join(", ",
        Enumerable.Range(1, arity).Select(i => $"Func<global::MultiType.NET.Core.Anys.Generated.Any<{string.Join(", ", Enumerable.Range(1, arity).Select(j => $"T{j}"))}>, T{i}, TResult> selector{i}"));

    var contextCases = string.Join(",\n            ",
        Enumerable.Range(1, arity).Select(i => $"{i} when Value is T{i} v{i} => selector{i}(this, v{i})"));

    sb.AppendLine($$"""
        /// <summary>
        /// Projects the Any value with access to full context.
        /// </summary>
        public TResult SelectWithContext<TResult>({{contextSelectors}})
        {
            return TypeIndex switch
            {
                {{contextCases}},
                _ => throw new InvalidAnyStateException("Any is not in a valid state for selection")
            };
        }
    """);

    // SelectWhere
    var predicates = string.Join(", ",
        Enumerable.Range(1, arity).Select(i => $"Func<T{i}, bool> predicate{i}, Func<T{i}, TResult> selector{i}"));

    var predicateCases = string.Join("\n        else ",
        Enumerable.Range(1, arity).Select(i => $$"""
            if (TypeIndex == {{i}} && Value is T{{i}} v{{i}} && predicate{{i}}(v{{i}}))
                return selector{{i}}(v{{i}});
        """));

    sb.AppendLine($$"""
        /// <summary>
        /// Conditionally maps Any value if predicate passes, otherwise returns default.
        /// </summary>
        public TResult SelectWhere<TResult>({{predicates}}, TResult defaultValue = default!)
        {
            {{predicateCases}}
            else return defaultValue;
        }
    """);

    // SelectAsync
    var asyncSelectors = string.Join(", ",
        Enumerable.Range(1, arity).Select(i => $"Func<T{i}, Task<TResult>> selector{i}"));

    var asyncCases = string.Join(",\n            ",
        Enumerable.Range(1, arity).Select(i => $"{i} when Value is T{i} v{i} => selector{i}(v{i})"));

    sb.AppendLine($$"""
        /// <summary>
        /// Asynchronously projects the Any value using the specified async selector functions.
        /// </summary>
        public async Task<TResult> SelectAsync<TResult>({{asyncSelectors}})
        {
            return await (TypeIndex switch
            {
                {{asyncCases}},
                _ => throw new InvalidAnyStateException("Any is not in a valid state for selection")
            });
        }
    """);

    // SelectAsyncOrDefault
    sb.AppendLine($$"""
        /// <summary>
        /// Asynchronously projects the Any value, or returns fallback default.
        /// </summary>
        public async Task<TResult> SelectAsyncOrDefault<TResult>({{asyncSelectors}}, TResult defaultValue = default!)
        {
            try { return await SelectAsync({{string.Join(", ", Enumerable.Range(1, arity).Select(i => $"selector{i}"))}}); }
            catch { return defaultValue; }
        }
    """);
}

}