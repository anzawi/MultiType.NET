namespace MultiType.NET.Core.Unions;

/// <inheritdoc />
public readonly struct Union<T1, T2, T3, T4, T5, T6> : IUnion
{
    private readonly ValueType? _valueType;
    private readonly object? _referenceType;
    private readonly bool _isValueType;

    /// <inheritdoc />
    public byte TypeIndex { get; }

    /// <summary>
    /// A union type that can hold a value of one of six predefined types.
    /// </summary>
    /// <typeparam name="T1">The first potential type the union can hold.</typeparam>
    /// <typeparam name="T2">The second potential type the union can hold.</typeparam>
    /// <typeparam name="T3">The third potential type the union can hold.</typeparam>
    /// <typeparam name="T4">The fourth potential type the union can hold.</typeparam>
    /// <typeparam name="T5">The fifth potential type the union can hold.</typeparam>
    /// <typeparam name="T6">The sixth potential type the union can hold.</typeparam>
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
    /// Represents a union type that can store a value of one of six possible types: T1, T2, T3, T4, T5, or T6.
    /// </summary>
    /// <typeparam name="T1">The first type in the union.</typeparam>
    /// <typeparam name="T2">The second type in the union.</typeparam>
    /// <typeparam name="T3">The third type in the union.</typeparam>
    /// <typeparam name="T4">The fourth type in the union.</typeparam>
    /// <typeparam name="T5">The fifth type in the union.</typeparam>
    /// <typeparam name="T6">The sixth type in the union.</typeparam>
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
    /// Represents a union type that can store a value of one of the six specified types.
    /// </summary>
    /// <typeparam name="T1">The first possible type of the union.</typeparam>
    /// <typeparam name="T2">The second possible type of the union.</typeparam>
    /// <typeparam name="T3">The third possible type of the union.</typeparam>
    /// <typeparam name="T4">The fourth possible type of the union.</typeparam>
    /// <typeparam name="T5">The fifth possible type of the union.</typeparam>
    /// <typeparam name="T6">The sixth possible type of the union.</typeparam>
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

    /// <summary>
    /// Represents a union type capable of holding a value of one of six predefined types: T1, T2, T3, T4, T5, or T6.
    /// </summary>
    /// <typeparam name="T1">The first possible type.</typeparam>
    /// <typeparam name="T2">The second possible type.</typeparam>
    /// <typeparam name="T3">The third possible type.</typeparam>
    /// <typeparam name="T4">The fourth possible type.</typeparam>
    /// <typeparam name="T5">The fifth possible type.</typeparam>
    /// <typeparam name="T6">The sixth possible type.</typeparam>
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

    /// <summary>
    /// Represents a union type that can hold a value of one of six predefined types.
    /// </summary>
    /// <typeparam name="T1">The first type of the union.</typeparam>
    /// <typeparam name="T2">The second type of the union.</typeparam>
    /// <typeparam name="T3">The third type of the union.</typeparam>
    /// <typeparam name="T4">The fourth type of the union.</typeparam>
    /// <typeparam name="T5">The fifth type of the union.</typeparam>
    /// <typeparam name="T6">The sixth type of the union.</typeparam>
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

    /// <summary>
    /// A generic union type that can hold a value of one of six predefined types.
    /// </summary>
    /// <typeparam name="T1">The first possible type.</typeparam>
    /// <typeparam name="T2">The second possible type.</typeparam>
    /// <typeparam name="T3">The third possible type.</typeparam>
    /// <typeparam name="T4">The fourth possible type.</typeparam>
    /// <typeparam name="T5">The fifth possible type.</typeparam>
    /// <typeparam name="T6">The sixth possible type.</typeparam>
    /// <remarks>
    /// Use the implicit conversion operators to seamlessly create instances of the union type from its predefined types.
    /// This structure is immutable and provides mechanisms to safely access the underlying value.
    /// </remarks>
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


    public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T1? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T2? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T3? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T4? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T5? v) => new(v);
    public static implicit operator Union<T1, T2, T3, T4, T5, T6>(T6? v) => new(v);

    public override string ToString() => this.Value?.ToString() ?? "null";
}