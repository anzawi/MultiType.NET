namespace MultiType.NET.Core.Unions;
/// <inheritdoc />
public readonly struct Union<T1, T2, T3, T4, T5, T6, T7> : IUnion
{
    private readonly ValueType? _valueType;
    private readonly object? _referenceType;
    private readonly bool _isValueType;

    public byte TypeIndex { get; }

    /// Represents a type-safe union that can store one of seven possible types.
    /// This structure provides the ability to handle multiple type values while maintaining a clear type identity at runtime.
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

    /// Represents a union type that can hold one of seven possible types.
    /// Useful for working with values that can be one of multiple types in a strongly-typed manner.
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

    /// Represents a union type that can hold one value from seven possible types.
    /// This type provides methods to check and retrieve the value in a type-safe manner
    /// while ensuring strict adherence to the designated types.
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

    /// Represents a union type that can hold a value of one of seven possible types.
    /// This is a generic, immutable structure that ensures exactly one of the seven types is selected
    /// and provides runtime access and type checking for the stored value.
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

    /// Represents a union type that can store a value of one of seven possible types.
    /// Provides operations to determine the stored type, retrieve the value, and cast to specific types.
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

    /// Represents a union of seven possible types, allowing a value to be one of the defined types (T1 through T7).
    /// This provides an efficient and type-safe way to handle multiple types in a single storage.
    /// Implements the IUnion interface.
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

    /// <summary>
    /// Represents a generic union type that can store one of seven possible types.
    /// </summary>
    /// <typeparam name="T1">The first possible type.</typeparam>
    /// <typeparam name="T2">The second possible type.</typeparam>
    /// <typeparam name="T3">The third possible type.</typeparam>
    /// <typeparam name="T4">The fourth possible type.</typeparam>
    /// <typeparam name="T5">The fifth possible type.</typeparam>
    /// <typeparam name="T6">The sixth possible type.</typeparam>
    /// <typeparam name="T7">The seventh possible type.</typeparam>
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


    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T1? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T2? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T3? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T4? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T5? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T6? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6, T7>(T7? v) => new(v);

    public override string ToString() => this.Value?.ToString() ?? "null";
}