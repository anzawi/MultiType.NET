//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by MultiType.NET.SourceGenerator
//     Library Version: 1.0.0.0
//     Runtime Version: 8.0.17
//     Generated: 2025-06-21 20:48:56 UTC
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace MultiType.NET.Core.Anys.Generated;
using global::System.Diagnostics.CodeAnalysis;
using global::System.Diagnostics;
using global::System.Runtime.CompilerServices;
using global::System.Text.Json.Serialization;
using global::MultiType.NET.Core.Helpers;
using global::System.Text.Json;
using global::MultiType.NET.Core.Exceptions;
using global::MultiType.NET.Core.Serialization.Generated;

/// <inheritdoc/>
[JsonConverter(typeof(AnyJsonConverter<, >))]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public readonly struct Any<T1, T2> : global::MultiType.NET.Core.IAny
{
    private string DebuggerDisplay => TypeIndex == 0 ? "[Uninitialized]" : $"[TypeIndex: {TypeIndex}] Value = {Value} ({Value?.GetType().Name})";

    private readonly ValueType? _valueType;
    private readonly object? _referenceType;
    private readonly bool _isValueType;
    /// <inheritdoc/>
    public byte TypeIndex { get; }
    /// <inheritdoc/>
    public object? Value => _isValueType ? _valueType : _referenceType;
    /// <inheritdoc/>
    public Type Type => Value?.GetType() ?? typeof(void);
    /// <inheritdoc/>
    public bool HasValue => this.Value is not null;
    /// <inheritdoc/>
    public bool IsNull => _valueType is null && _referenceType is null;
    /// <summary>
    /// The set of allowed types for this Any.
    /// </summary>
    public static Type[] AllowedTypes => new[]
    {
        typeof(T1),
        typeof(T2)
    };

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Is<T>() => (typeof(T) == typeof(T1) && TypeIndex == 1) || (typeof(T) == typeof(T2) && TypeIndex == 2);
    /// <inheritdoc/>
    public T As<T>()
    {
        if (Is<T>())
            return (T)Value!;
        throw new InvalidCastException($"Type {typeof(T).Name} is not stored in this Any<{typeof(T1).Name}, {typeof(T2).Name}>.");
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? AsNullable<T>()
        where T : class
    {
        if (IsNull)
            return null;
        return As<T>();
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? AsNullableStruct<T>()
        where T : struct
    {
        if (IsNull)
            return null;
        return As<T>();
    }

    /// <summary>
    /// Creates a new Any from the given value.
    /// </summary>
    /// <remarks>
    /// ⚠️ <b>Performance Warning:</b>
    /// This method causes boxing of value types. For best performance, use <c>FromTn</c>, or implicit casting instead.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Any<T1, T2> From(object? value)
    {
        return value switch
        {
            T1 v1 => new Any<T1, T2>(v1),
            T2 v2 => new Any<T1, T2>(v2),
            _ => throw new InvalidCastException($"Cannot cast {value?.GetType().Name ?? "null"} to Any of ({typeof(T1).Name}, {typeof(T2).Name})")};
    }

    /// <summary>
    /// Creates a new Any from the given value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFrom(object? value, out Any<T1, T2> result)
    {
        switch (value)
        {
            case T1 v1:
                result = new Any<T1, T2>(v1);
                return true;
            case T2 v2:
                result = new Any<T1, T2>(v2);
                return true;
            default:
                result = default;
                return false;
        }
    }

    /// <summary>
    /// Creates a new Any from the given value of type T1.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Any<T1, T2> FromT1(T1? value)
    {
        return new Any<T1, T2>(value);
    }

    /// <summary>
    /// Creates a new Any from the given value of type T2.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Any<T1, T2> FromT2(T2? value)
    {
        return new Any<T1, T2>(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Any(T1? value)
    {
        TypeIndex = 1;
        if (value is null)
        {
            _valueType = null;
            _referenceType = null;
            _isValueType = false;
            return;
        }

        if (value is ValueType vt)
        {
            _valueType = vt;
            _referenceType = null;
            _isValueType = true;
        }
        else
        {
            _valueType = null;
            _referenceType = value;
            _isValueType = false;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Any(T2? value)
    {
        TypeIndex = 2;
        if (value is null)
        {
            _valueType = null;
            _referenceType = null;
            _isValueType = false;
            return;
        }

        if (value is ValueType vt)
        {
            _valueType = vt;
            _referenceType = null;
            _isValueType = true;
        }
        else
        {
            _valueType = null;
            _referenceType = value;
            _isValueType = false;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Any<T1, T2>(T1? v) => new(v);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Any<T1, T2>(T2? v) => new(v);
    /// <summary>
    /// Returns a string representation of the current Any value.
    /// </summary>
    public override string ToString()
    {
        if (TypeIndex == 0)
            return "Any[Uninitialized]";
        string typeName = Value?.GetType().Name ?? "null";
        string valueStr = Value?.ToString() ?? "null";
        return $"Any[{typeName}] = {valueStr}";
    }

    public override bool Equals(object? obj)
    {
        // Fast path: Reference equality
        if (ReferenceEquals(this, obj))
            return true;
        // Check exact type (prevent cross-type comparison)
        if (obj is not global::MultiType.NET.Core.Anys.Generated.Any<T1, T2> other)
            return false;
        // Fast path: Same index and both null
        if (TypeIndex == other.TypeIndex && Value is null && other.Value is null)
            return true;
        // Full equality check
        return TypeIndex == other.TypeIndex && Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        // Recommended pattern: HashCode.Combine (C# 8+)
        return HashCode.Combine(TypeIndex, Value);
    }

    public static bool operator ==(global::MultiType.NET.Core.Anys.Generated.Any<T1, T2> left, global::MultiType.NET.Core.Anys.Generated.Any<T1, T2> right) => left.Equals(right);
    public static bool operator !=(global::MultiType.NET.Core.Anys.Generated.Any<T1, T2> left, global::MultiType.NET.Core.Anys.Generated.Any<T1, T2> right) => !left.Equals(right);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Match<TResult>(Func<T1, TResult> case1, Func<T2, TResult> case2)
    {
        return this.TypeIndex switch
        {
            1 => case1((T1)Value!),
            2 => case2((T2)Value!),
            _ => throw new InvalidOperationException("Any is not initialized."),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Match([MaybeNull] Action<T1> case1, [MaybeNull] Action<T2> case2)
    {
        switch (this.TypeIndex)
        {
            case 1:
                case1((T1)Value!);
                break;
            case 2:
                case2((T2)Value!);
                break;
            default:
                throw new InvalidOperationException("Any is not initialized.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult? TryMatch<TResult>(Func<T1, TResult>? case1 = null, Func<T2, TResult>? case2 = null)
    {
        return this.TypeIndex switch
        {
            1when case1 != null => case1((T1)Value!),
            2when case2 != null => case2((T2)Value!),
            _ => default,
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void TryMatch([MaybeNull] Action<T1>? case1 = null, [MaybeNull] Action<T2>? case2 = null)
    {
        switch (this.TypeIndex)
        {
            case 1when case1 != null:
                case1((T1)Value!);
                break;
            case 2when case2 != null:
                case2((T2)Value!);
                break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T1 GetT1()
    {
        try
        {
            Guards.ThrowIfNotInitialized(TypeIndex, typeof(T1));
            Guards.ThrowIfNotOutOfRange(TypeIndex, 2);
            Guards.ThrowIfNull(Value, typeof(T1), TypeIndex);
            Guards.ThrowIfTypeMismatch<T1>(TypeIndex, 1, Value, out T1 typedValue);
            return typedValue;
        }
        catch (Exception ex)when (ex is not InvalidAnyStateException)
        {
            // Wrap unexpected exceptions (Better Exception Handling)
            throw new InvalidAnyStateException($"Unexpected error getting value of type {typeof(T1).Name}\nTypeIndex: {TypeIndex}\nValue type: {Value?.GetType().Name ?? "null"}", ex);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T2 GetT2()
    {
        try
        {
            Guards.ThrowIfNotInitialized(TypeIndex, typeof(T2));
            Guards.ThrowIfNotOutOfRange(TypeIndex, 2);
            Guards.ThrowIfNull(Value, typeof(T2), TypeIndex);
            Guards.ThrowIfTypeMismatch<T2>(TypeIndex, 2, Value, out T2 typedValue);
            return typedValue;
        }
        catch (Exception ex)when (ex is not InvalidAnyStateException)
        {
            // Wrap unexpected exceptions (Better Exception Handling)
            throw new InvalidAnyStateException($"Unexpected error getting value of type {typeof(T2).Name}\nTypeIndex: {TypeIndex}\nValue type: {Value?.GetType().Name ?? "null"}", ex);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>([MaybeNull] out T value)
    {
        if (Is<T>())
        {
            value = As<T>();
            return true;
        }

        value = default !;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetT1([MaybeNull] out T1 value)
    {
        value = default;
        if (TypeIndex == 1 && Value is T1 t1)
        {
            value = t1;
            return true;
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetT2([MaybeNull] out T2 value)
    {
        value = default;
        if (TypeIndex == 2 && Value is T2 t2)
        {
            value = t2;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Maps the value contained in this Any to a new type using a common result type.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult Map<TResult>(Func<T1, TResult> map1, Func<T2, TResult> map2)
    {
        return TypeIndex switch
        {
            1when Value is T1 v1 => map1(v1),
            2when Value is T2 v2 => map2(v2),
            _ => throw new InvalidAnyStateException("Any is not in a valid state for mapping")};
    }

    /// <summary>
    /// Maps the value to a new Any type with different type parameters.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public global::MultiType.NET.Core.Anys.Generated.Any<TResult1, TResult2> MapAny<TResult1, TResult2>(Func<T1, TResult1> map1, Func<T2, TResult2> map2)
    {
        return TypeIndex switch
        {
            1when Value is T1 v1 => map1(v1),
            2when Value is T2 v2 => map2(v2),
            _ => throw new InvalidAnyStateException("Any is not in a valid state for mapping")};
    }

    /// <summary>
    /// Maps the value using different strategies for value types and reference types.
    /// </summary>
    public TResult MapValue<TResult>(Func<ValueType, TResult> valueTypeMapper, Func<object, TResult> referenceTypeMapper)
    {
        if (Value is null)
            throw new InvalidAnyStateException("Cannot map null value");
        return _isValueType ? valueTypeMapper(_valueType!) : referenceTypeMapper(_referenceType!);
    }

    /// <summary>
    /// Asynchronously maps the Any value.
    /// </summary>
    public async Task<TResult> MapAsync<TResult>(Func<T1, Task<TResult>> map1, Func<T2, Task<TResult>> map2)
    {
        return TypeIndex switch
        {
            1when Value is T1 v1 => await map1(v1),
            2when Value is T2 v2 => await map2(v2),
            _ => throw new InvalidAnyStateException("Any is not in a valid state for mapping")};
    }

    /// <summary>
    /// Maps the value with null-safety, returning a default value if null.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult MapOrDefault<TResult>(Func<T1, TResult> map1, Func<T2, TResult> map2, TResult defaultValue = default !)
    {
        if (IsNull)
            return defaultValue;
        return TypeIndex switch
        {
            1when Value is T1 v1 => map1(v1),
            2when Value is T2 v2 => map2(v2),
            _ => defaultValue
        };
    }

    /// <summary>
    /// Maps the value with exception handling.
    /// </summary>
    public TResult MapSafe<TResult>(Func<T1, TResult> map1, Func<T2, TResult> map2, Func<Exception, TResult> errorHandler)
    {
        try
        {
            return TypeIndex switch
            {
                1when Value is T1 v1 => map1(v1),
                2when Value is T2 v2 => map2(v2),
                _ => throw new InvalidAnyStateException("Any is not in a valid state for mapping")};
        }
        catch (Exception ex)
        {
            return errorHandler(ex);
        }
    }

    /// <summary>
    /// Maps the value conditionally based on predicates.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult MapWhere<TResult>(Func<T1, bool> predicate1, Func<T1, TResult> map1, Func<T2, bool> predicate2, Func<T2, TResult> map2, TResult defaultValue = default !)
    {
        return TypeIndex switch
        {
            1when Value is T1 v1 && predicate1(v1) => map1(v1),
            2when Value is T2 v2 && predicate2(v2) => map2(v2),
            _ => defaultValue
        };
    }

    /// <summary>
    /// Maps with access to the entire Any instance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TResult MapWithContext<TResult>(Func<global::MultiType.NET.Core.Anys.Generated.Any<T1, T2>, T1, TResult> map1, Func<global::MultiType.NET.Core.Anys.Generated.Any<T1, T2>, T2, TResult> map2)
    {
        return TypeIndex switch
        {
            1when Value is T1 v1 => map1(this, v1),
            2when Value is T2 v2 => map2(this, v2),
            _ => throw new InvalidAnyStateException("Any is not in a valid state for mapping")};
    }

    /// <summary>
    /// Projects the Any value into a new form using the specified selector functions.
    /// </summary>
    public TResult Select<TResult>(Func<T1, TResult> selector1, Func<T2, TResult> selector2)
    {
        return TypeIndex switch
        {
            1when Value is T1 v1 => selector1(v1),
            2when Value is T2 v2 => selector2(v2),
            _ => throw new InvalidAnyStateException("Any is not in a valid state for selection")};
    }

    /// <summary>
    /// Projects the Any value using selector functions, or returns a fallback default.
    /// </summary>
    public TResult SelectOrDefault<TResult>(Func<T1, TResult> selector1, Func<T2, TResult> selector2, TResult defaultValue = default !)
    {
        try
        {
            return Select(selector1, selector2);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Attempts to project the Any value using selector functions.
    /// Returns null if uninitialized or type mismatch.
    /// </summary>
    public TResult? TrySelect<TResult>(Func<T1, TResult> selector1, Func<T2, TResult> selector2)
    {
        if (TypeIndex == 0 || Value is null)
            return default;
        return Select(selector1, selector2);
    }

    /// <summary>
    /// Projects the Any value with access to full context.
    /// </summary>
    public TResult SelectWithContext<TResult>(Func<global::MultiType.NET.Core.Anys.Generated.Any<T1, T2>, T1, TResult> selector1, Func<global::MultiType.NET.Core.Anys.Generated.Any<T1, T2>, T2, TResult> selector2)
    {
        return TypeIndex switch
        {
            1when Value is T1 v1 => selector1(this, v1),
            2when Value is T2 v2 => selector2(this, v2),
            _ => throw new InvalidAnyStateException("Any is not in a valid state for selection")};
    }

    /// <summary>
    /// Conditionally maps Any value if predicate passes, otherwise returns default.
    /// </summary>
    public TResult SelectWhere<TResult>(Func<T1, bool> predicate1, Func<T1, TResult> selector1, Func<T2, bool> predicate2, Func<T2, TResult> selector2, TResult defaultValue = default !)
    {
        if (TypeIndex == 1 && Value is T1 v1 && predicate1(v1))
            return selector1(v1);
        else if (TypeIndex == 2 && Value is T2 v2 && predicate2(v2))
            return selector2(v2);
        else
            return defaultValue;
    }

    /// <summary>
    /// Asynchronously projects the Any value using the specified async selector functions.
    /// </summary>
    public async Task<TResult> SelectAsync<TResult>(Func<T1, Task<TResult>> selector1, Func<T2, Task<TResult>> selector2)
    {
        return await (TypeIndex switch
        {
            1when Value is T1 v1 => selector1(v1),
            2when Value is T2 v2 => selector2(v2),
            _ => throw new InvalidAnyStateException("Any is not in a valid state for selection")});
    }

    /// <summary>
    /// Asynchronously projects the Any value, or returns fallback default.
    /// </summary>
    public async Task<TResult> SelectAsyncOrDefault<TResult>(Func<T1, Task<TResult>> selector1, Func<T2, Task<TResult>> selector2, TResult defaultValue = default !)
    {
        try
        {
            return await SelectAsync(selector1, selector2);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Executes the appropriate action based on the Any type.
    /// </summary>
    public void Switch(Action<T1> case1, Action<T2> case2)
    {
        switch (TypeIndex)
        {
            case 1when Value is T1 v1:
                case1(v1);
                break;
            case 2when Value is T2 v2:
                case2(v2);
                break;
            default:
                throw new InvalidAnyStateException("Any is not in a valid state for switching");
        };
    }

    /// <summary>
    /// Asynchronously executes the appropriate action based on the Any type.
    /// </summary>
    public async Task SwitchAsync(Func<T1, Task> case1, Func<T2, Task> case2)
    {
        switch (TypeIndex)
        {
            case 1when Value is T1 v1:
                await case1(v1);
                break;
            case 2when Value is T2 v2:
                await case2(v2);
                break;
            default:
                throw new InvalidAnyStateException("Any is not in a valid state for switching");
        }
    }

    /// <summary>
    /// Executes the appropriate action if the Any is initialized; otherwise, runs default fallback.
    /// </summary>
    public void SwitchOrDefault(Action<T1> case1, Action<T2> case2, Action fallback)
    {
        switch (TypeIndex)
        {
            case 1when Value is T1 v1:
                case1(v1);
                break;
            case 2when Value is T2 v2:
                case2(v2);
                break;
            default:
                fallback();
                break;
        }
    }

    /// <summary>
    /// Deconstructs the Any into individual out variables based on the active type.
    /// </summary>
    public void Deconstruct(out T1 value1, out T2 value2)
    {
        value1 = default !;
        value2 = default !;
        switch (TypeIndex)
        {
            case 1when Value is T1 v1:
                value1 = v1;
                break;
            case 2when Value is T2 v2:
                value2 = v2;
                break;
            default:
                throw new InvalidAnyStateException($"""
                Unable to deconstruct Any — invalid TypeIndex ({TypeIndex}) or type mismatch.
                Actual value: {Value?.GetType().Name ?? "null"}
            """);
        }
    }

    public static bool TryParse(string input, IFormatProvider? _, out global::MultiType.NET.Core.Anys.Generated.Any<T1, T2> result)
    {
        try
        {
            if (TryCast<T1>(input, out var t1))
            {
                result = FromT1(t1);
                return true;
            }

            if (TryCast<T2>(input, out var t2))
            {
                result = FromT2(t2);
                return true;
            }
        }
        catch
        {
        // ignore
        }

        result = default;
        return false;
    }

    public static bool TryCast<T>(string? input, out T? value)
    {
        value = default;
        if (typeof(T).IsPrimitiveType(input, out var parsed))
        {
            value = (T)parsed!;
            return true;
        }

        try
        {
            bool needsQuotes = typeof(T) == typeof(string) && (input?.StartsWith("\"") != true && input?.EndsWith("\"") != true);
            string jsonInput = needsQuotes ? $"\"{input}\"" : input!;
            value = JsonSerializer.Deserialize<T>(jsonInput);
            return value is not null;
        }
        catch
        {
            return false;
        }
    }
}