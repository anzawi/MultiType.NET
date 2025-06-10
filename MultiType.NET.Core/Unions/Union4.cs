namespace MultiType.NET.Core.Unions;

using Exceptions;
using Helpers;

/// <inheritdoc />
public readonly struct Union<T1, T2, T3, T4> : IUnion
{
    private readonly ValueType? _valueType;
    private readonly object? _referenceType;
    private readonly bool _isValueType;
    public byte TypeIndex { get; }

    /// Represents a generic union type capable of holding a value of one of four possible types.
    /// Provides utilities for type discrimination and type casting.
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
    /// Represents a union type that can hold one of four different types of values
    /// specified by <typeparamref name="T1"/>, <typeparamref name="T2"/>,
    /// <typeparamref name="T3"/>, or <typeparamref name="T4"/>.
    /// </summary>
    /// <typeparam name="T1">The first potential type of the value.</typeparam>
    /// <typeparam name="T2">The second potential type of the value.</typeparam>
    /// <typeparam name="T3">The third potential type of the value.</typeparam>
    /// <typeparam name="T4">The fourth potential type of the value.</typeparam>
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

    /// <summary>
    /// Represents a discriminated union of four potential types. Instances of this type
    /// can hold a value of one of four specified types and provide mechanisms to retrieve
    /// the value and its type.
    /// </summary>
    /// <typeparam name="T1">The first possible type.</typeparam>
    /// <typeparam name="T2">The second possible type.</typeparam>
    /// <typeparam name="T3">The third possible type.</typeparam>
    /// <typeparam name="T4">The fourth possible type.</typeparam>
    public Union(T3? value)
    {
        this.TypeIndex = 3;
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

    /// Represents a union type that can hold a value of one of four different types.
    /// This is a generic structure that provides type-safe handling of multiple types,
    /// allowing for operations and conversions while maintaining type safety.
    /// <typeparam name="T1">The first possible type of the value.</typeparam>
    /// <typeparam name="T2">The second possible type of the value.</typeparam>
    /// <typeparam name="T3">The third possible type of the value.</typeparam>
    /// <typeparam name="T4">The fourth possible type of the value.</typeparam>
    public Union(T4? value)
    {
        this.TypeIndex = 4;
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
    public T As<T>() =>
        this.Value is T val
            ? val
            : throw new InvalidCastException(
                $"Cannot cast union value of type {this.Value?.GetType().Name ?? "null"} to {typeof(T).Name}");

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

    public T1 GetT1(out Union<T2, T3, T4>? remainder)
    {
        remainder = default;
        try
        {
            Guards.ThrowIfNotInitialized(TypeIndex, typeof(T1));
            Guards.ThrowIfNotOutOfRange(TypeIndex, 4);
            Guards.ThrowIfNull(Value, typeof(T1), TypeIndex);
            Guards.ThrowIfTypeMismatch<T1>(TypeIndex, 1, Value, out T1 typedValue);

            return typedValue;
        }
        catch (Exception ex) when (ex is not InvalidUnionStateException)
        {
            throw new InvalidUnionStateException(
                $"""
                 Unexpected error getting value of type {typeof(T1).Name}
                 TypeIndex: {TypeIndex}
                 Value type: {Value?.GetType().Name ?? "null"}
                 """, ex);
        }
    }

    public T2 GetT2(out Union<T1, T3, T4>? remainder)
    {
        remainder = default;
        try
        {
            Guards.ThrowIfNotInitialized(TypeIndex, typeof(T2));
            Guards.ThrowIfNotOutOfRange(TypeIndex, 4);
            Guards.ThrowIfNull(Value, typeof(T2), TypeIndex);
            Guards.ThrowIfTypeMismatch<T2>(TypeIndex, 2, Value, out T2 typedValue);

            return typedValue;
        }
        catch (Exception ex) when (ex is not InvalidUnionStateException)
        {
            throw new InvalidUnionStateException(
                $"""
                 Unexpected error getting value of type {typeof(T2).Name}
                 TypeIndex: {TypeIndex}
                 Value type: {Value?.GetType().Name ?? "null"}
                 """, ex);
        }
    }

    public T3 GetT3(out Union<T1, T2, T4>? remainder)
    {
        remainder = default;
        try
        {
            Guards.ThrowIfNotInitialized(TypeIndex, typeof(T3));
            Guards.ThrowIfNotOutOfRange(TypeIndex, 4);
            Guards.ThrowIfNull(Value, typeof(T3), TypeIndex);
            Guards.ThrowIfTypeMismatch<T3>(TypeIndex, 3, Value, out T3 typedValue);

            return typedValue;
        }
        catch (Exception ex) when (ex is not InvalidUnionStateException)
        {
            throw new InvalidUnionStateException(
                $"""
                 Unexpected error getting value of type {typeof(T3).Name}
                 TypeIndex: {TypeIndex}
                 Value type: {Value?.GetType().Name ?? "null"}
                 """, ex);
        }
    }

    public T4 GetT4(out Union<T1, T2, T3>? remainder)
    {
        remainder = default;
        try
        {
            Guards.ThrowIfNotInitialized(TypeIndex, typeof(T4));
            Guards.ThrowIfNotOutOfRange(TypeIndex, 4);
            Guards.ThrowIfNull(Value, typeof(T4), TypeIndex);
            Guards.ThrowIfTypeMismatch<T4>(TypeIndex, 4, Value, out T4 typedValue);

            return typedValue;
        }
        catch (Exception ex) when (ex is not InvalidUnionStateException)
        {
            throw new InvalidUnionStateException(
                $"""
                 Unexpected error getting value of type {typeof(T4).Name}
                 TypeIndex: {TypeIndex}
                 Value type: {Value?.GetType().Name ?? "null"}
                 """, ex);
        }
    }

// TryGet Methods
    public bool TryGetT1(out T1 value, out Union<T2, T3, T4>? remainder)
    {
        value = default!;
        remainder = default;
        if (TypeIndex != 1 || Value is not T1 typedValue)
            return false;
        value = typedValue;
        return true;
    }

    public bool TryGetT2(out T2 value, out Union<T1, T3, T4>? remainder)
    {
        value = default!;
        remainder = default;
        if (TypeIndex != 2 || Value is not T2 typedValue)
            return false;
        value = typedValue;
        return true;
    }

    public bool TryGetT3(out T3 value, out Union<T1, T2, T4>? remainder)
    {
        value = default!;
        remainder = default;
        if (TypeIndex != 3 || Value is not T3 typedValue)
            return false;
        value = typedValue;
        return true;
    }

    public bool TryGetT4(out T4 value, out Union<T1, T2, T3>? remainder)
    {
        value = default!;
        remainder = default;
        if (TypeIndex != 4 || Value is not T4 typedValue)
            return false;
        value = typedValue;
        return true;
    }


    /// <summary>
    /// Defines an implicit conversion for a value of type <typeparamref name="T1"/> to a <see cref="Union{T1, T2, T3, T4}"/>.
    /// </summary>
    /// <param name="v">The value to convert.</param>
    /// <returns>A new <see cref="Union{T1, T2, T3, T4}"/> containing the specified value.</returns>
    public static implicit operator Union<T1, T2, T3, T4>(T1? v) => new(v);

    /// <summary>
    /// Defines an implicit operator to create a <see cref="Union{T1, T2, T3, T4}"/> from a value of type <typeparamref name="T2"/>.
    /// </summary>
    /// <param name="v">The value of type <typeparamref name="T2"/> to be wrapped in the union.</param>
    /// <returns>A <see cref="Union{T1, T2, T3, T4}"/> instance containing the specified value.</returns>
    public static implicit operator Union<T1, T2, T3, T4>(T2? v) => new(v);

    /// <summary>
    /// Defines an implicit conversion operator to create a <see cref="Union{T1, T2, T3, T4}"/> instance
    /// from a value of type <typeparamref name="T3"/>.
    /// </summary>
    /// <param name="v">The value of type <typeparamref name="T3"/> to be wrapped in the union.</param>
    /// <returns>A <see cref="Union{T1, T2, T3, T4}"/> instance containing the specified value.</returns>
    public static implicit operator Union<T1, T2, T3, T4>(T3? v) => new(v);

    /// Provides an implicit conversion from a nullable value of type <typeparamref name="T4"/>
    /// to a union instance of type <see cref="Union{T1, T2, T3, T4}"/>.
    public static implicit operator Union<T1, T2, T3, T4>(T4? v) => new(v);

    /// <summary>
    /// Returns the string representation of the value contained in the union.
    /// If the union is null, returns "null".
    /// </summary>
    /// <returns>
    /// The string representation of the value contained in the union, or "null" if the union is null.
    /// </returns>
    public override string ToString() => this.Value?.ToString() ?? "null";
}