namespace MultiType.NET.Core.Unions;

/// <inheritdoc />
public readonly struct Union<T1, T2, T3, T4, T5, T6, T7, T8> : IUnion
{
    private readonly ValueType? _valueType;
    private readonly object? _referenceType;
    private readonly bool _isValueType;

    /// <inheritdoc />
    public byte TypeIndex { get; }

    /// Represents a value that can be one of eight different types.
    /// This struct provides a way to hold a value of one type, among a specified set of types,
    /// while preserving type information and providing utility methods for working with the value.
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

    /// Represents a discriminated union type capable of holding one of eight possible types.
    /// This struct provides a way to encapsulate multiple types into a single variable while maintaining type safety.
    /// It supports implicit conversion from the generic types and provides utilities for type checking and casting.
    /// The underlying value is stored as a `ValueType?` or `object?`, distinguished by a type index.
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

    /// Represents a type-safe container capable of holding one of eight different types of values.
    /// The type of value stored is identified by the `TypeIndex` property.
    /// This struct serves as an implementation of a discriminated union, allowing flexible handling of multiple types
    /// while maintaining type safety.
    /// Implements the `IUnion` interface to provide standardized access to union types and values.
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

    /// Represents a union of up to eight possible types.
    /// This struct allows an instance to contain a value of one of its specified types.
    /// The actual type of the value is identified by the `TypeIndex` property.
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

    /// Represents a union type that can hold an instance of one of eight different types.
    /// Provides functionality to determine the type currently held, retrieve its value, and perform type-guarded operations.
    public Union(T5? value)
    {
        this.TypeIndex = 5;
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

    /// Represents a union type that can hold a value of one of the eight specified types.
    /// Provides type-safe access to the encapsulated value while maintaining information about the type of value stored.
    public Union(T6? value)
    {
        this.TypeIndex = 6;
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

    /// Represents a union type capable of holding one of eight different types.
    /// This struct provides functionality to store and interact with values of different types
    /// while maintaining type safety and avoiding boxing/unboxing where possible.
    public Union(T7? value)
    {
        this.TypeIndex = 7;
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

    /// Represents a union type that can hold one of eight possible types.
    /// This structure provides a way to handle values of several different types in a single variable.
    /// It implements the IUnion interface that defines the shared behavior and properties of union types.
    public Union(T8? value)
    {
        this.TypeIndex = 8;
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

    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T1? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T2? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T3? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T4? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T5? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T6? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T7? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7, T8>(T8? v) => new(v);

    public override string ToString() => this.Value?.ToString() ?? "null";
}