namespace MultiType.NET.Core.Unions;

using Exceptions;
using Helpers;

/// <inheritdoc />
public readonly struct Union<T1, T2> : IUnion
{
    private readonly ValueType? _valueType;
    private readonly object? _referenceType;
    private readonly bool _isValueType;

    /// <inheritdoc />
    public byte TypeIndex { get; }

    /// <summary>
    /// Represents a union type that can hold a value of one of the two specified types.
    /// </summary>
    /// <typeparam name="T1">The first type the union can hold.</typeparam>
    /// <typeparam name="T2">The second type the union can hold.</typeparam>
    public Union(T1? value)
    {
        this.TypeIndex = 1;
        if (value is null)
        {
            this._valueType = null;
            this._referenceType = null;
            this._isValueType = false;
            return;
        }

        if (value is ValueType vt)
        {
            this._valueType = vt;
            this._referenceType = null;
            this._isValueType = true;
        }
        else
        {
            this._valueType = null;
            this._referenceType = value;
            this._isValueType = false;
        }
    }

    /// <summary>
    /// Represents a union type capable of holding a value of one of two possible types.
    /// </summary>
    /// <typeparam name="T1">The first type that the union can hold.</typeparam>
    /// <typeparam name="T2">The second type that the union can hold.</typeparam>
    public Union(T2? value)
    {
        this.TypeIndex = 2;
        if (value is null)
        {
            this._valueType = null;
            this._referenceType = null;
            this._isValueType = false;
            return;
        }

        if (value is ValueType vt)
        {
            this._valueType = vt;
            this._referenceType = null;
            this._isValueType = true;
        }
        else
        {
            this._valueType = null;
            this._referenceType = value;
            this._isValueType = false;
        }
    }

    /// <inheritdoc />
    public object? Value => this._isValueType ? this._valueType : this._referenceType;

    /// <inheritdoc />
    public Type Type => this.Value?.GetType() ?? typeof(void);

    /// <inheritdoc />
    public bool IsNull => this._valueType is null && this._referenceType is null;

    /// <inheritdoc />
    public bool Is<T>() => this.Value is T;

    /// <inheritdoc />
    public T As<T>()
    {
        if (typeof(T) == typeof(T1) && this.TypeIndex == 1 ||
            typeof(T) == typeof(T2) && this.TypeIndex == 2)
        {
            return this.Value is T val
                ? val
                : throw new InvalidCastException(
                    $"Cannot cast union value of type {this.Value?.GetType().Name ?? "null"} to {typeof(T).Name}");
        }

        throw new InvalidCastException(
            $"Type {typeof(T).Name} is not one of the union type parameters");
    }

    /// <inheritdoc />
    public T? AsNullable<T>() where T : class
    {
        if (this.IsNull) return null;
        return this.As<T>();
    }

    /// <inheritdoc />
    public T? AsNullableStruct<T>() where T : struct
    {
        if (this.IsNull) return null;
        return this.As<T>();
    }


    public T1 GetT1(out Union<T2>? remainder)
    {
        // Initialize out parameter early (Defensive Programming)
        remainder = default;

        try
        {
            Guards.ThrowIfNotInitialized(TypeIndex, typeof(T1));
            Guards.ThrowIfNotOutOfRange(TypeIndex, 2);
            Guards.ThrowIfNull(Value, typeof(T1), TypeIndex);
            Guards.ThrowIfTypeMismatch<T1>(TypeIndex, 1, Value, out T1 typedValue);

            return typedValue;
        }
        catch (Exception ex) when (ex is not InvalidUnionStateException)
        {
            // Wrap unexpected exceptions (Better Exception Handling)
            throw new InvalidUnionStateException(
                $"""
                 Unexpected error getting value of type {typeof(T1).Name}
                 TypeIndex: {TypeIndex}
                 Value type: {Value?.GetType().Name ?? "null"}
                 """,
                ex);
        }
    }

    public T2 GetT2(out Union<T1>? remainder)
    {
        remainder = default;

        try
        {
            Guards.ThrowIfNotInitialized(TypeIndex, typeof(T2));
            Guards.ThrowIfNotOutOfRange(TypeIndex, 2);
            Guards.ThrowIfNull(Value, typeof(T2), TypeIndex);
            Guards.ThrowIfTypeMismatch<T2>(TypeIndex, 1, Value, out T2 typedValue);

            return typedValue;
        }
        catch (Exception ex) when (ex is not InvalidUnionStateException)
        {
            throw new InvalidUnionStateException(
                $"""
                 Unexpected error getting value of type {typeof(T2).Name}
                 TypeIndex: {TypeIndex}
                 Value type: {Value?.GetType().Name ?? "null"}
                 """,
                ex);
        }
    }

    public bool TryGetT1(out T1 value, out Union<T2>? remainder)
    {
        // Initialize out parameters early (Defensive Programming)
        value = default!;
        remainder = default;

        // Early exit for obvious failures (Performance)
        if (TypeIndex != 1 || Value is null)
            return false;

        // Type checking with pattern matching (Type Safety)
        if (Value is not T1 typedValue)
            return false;

        value = typedValue;
        return true;
    }

    public bool TryGetT2(out T2 value, out Union<T1>? remainder)
    {
        value = default!;
        remainder = default;

        if (TypeIndex != 2 || Value is null)
            return false;

        if (Value is not T2 typedValue)
            return false;

        value = typedValue;
        return true;
    }


    /// <summary>
    /// Defines an implicit conversion operator for creating a union instance from a value of the first type.
    /// </summary>
    /// <param name="value">The value of the first type to be converted into the union.</param>
    /// <returns>An instance of the <see cref="Union{T1, T2}"/> containing the specified value.</returns>
    public static implicit operator Union<T1, T2>(T1? value) => new(value);

    /// <summary>
    /// Defines an implicit conversion operation for creating a union instance from a given value of the second type.
    /// </summary>
    /// <param name="value">The value of the second type to convert to a union.</param>
    public static implicit operator Union<T1, T2>(T2? value) => new(value);

    /// <summary>
    /// Returns the string representation of the value held by the union or "null" if the value is null.
    /// </summary>
    /// <returns>The string representation of the contained value, or "null" if the union does not contain a value.</returns>
    public override string ToString() => this.Value?.ToString() ?? "null";
}