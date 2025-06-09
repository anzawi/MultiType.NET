namespace MultiType.NET.Core.Unions;

/// <inheritdoc />
public readonly struct Union<T1, T2, T3, T4, T5> : IUnion
{
    private readonly ValueType? _valueType;
    private readonly object? _referenceType;
    private readonly bool _isValueType;

    /// <inheritdoc />
    public byte TypeIndex { get; }

    /// <summary>
    /// Represents a union type that can hold a value of one of five predefined types.
    /// </summary>
    /// <typeparam name="T1">The first possible type of the union.</typeparam>
    /// <typeparam name="T2">The second possible type of the union.</typeparam>
    /// <typeparam name="T3">The third possible type of the union.</typeparam>
    /// <typeparam name="T4">The fourth possible type of the union.</typeparam>
    /// <typeparam name="T5">The fifth possible type of the union.</typeparam>
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
    /// Represents a union type that can hold one of five possible types.
    /// A readonly struct implementation is provided for lightweight, immutable usage.
    /// </summary>
    /// <typeparam name="T1">The first possible type the union can hold.</typeparam>
    /// <typeparam name="T2">The second possible type the union can hold.</typeparam>
    /// <typeparam name="T3">The third possible type the union can hold.</typeparam>
    /// <typeparam name="T4">The fourth possible type the union can hold.</typeparam>
    /// <typeparam name="T5">The fifth possible type the union can hold.</typeparam>
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
    /// Represents a union type that can hold a value of one of five predefined types.
    /// </summary>
    /// <typeparam name="T1">The first data type that the union can hold.</typeparam>
    /// <typeparam name="T2">The second data type that the union can hold.</typeparam>
    /// <typeparam name="T3">The third data type that the union can hold.</typeparam>
    /// <typeparam name="T4">The fourth data type that the union can hold.</typeparam>
    /// <typeparam name="T5">The fifth data type that the union can hold.</typeparam>
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
    /// Represents a discriminated union that can hold a value of one of five specified types.
    /// </summary>
    /// <typeparam name="T1">The first possible value type.</typeparam>
    /// <typeparam name="T2">The second possible value type.</typeparam>
    /// <typeparam name="T3">The third possible value type.</typeparam>
    /// <typeparam name="T4">The fourth possible value type.</typeparam>
    /// <typeparam name="T5">The fifth possible value type.</typeparam>
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
    /// Represents a union type that can store a value of one of five possible types.
    /// </summary>
    /// <typeparam name="T1">The first possible type.</typeparam>
    /// <typeparam name="T2">The second possible type.</typeparam>
    /// <typeparam name="T3">The third possible type.</typeparam>
    /// <typeparam name="T4">The fourth possible type.</typeparam>
    /// <typeparam name="T5">The fifth possible type.</typeparam>
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

    /// <inheritdoc />
    public object? Value => this._isValueType ? this._valueType : this._referenceType;

    /// <inheritdoc />
    public Type Type => this.Value?.GetType() ?? typeof(void);

    /// <inheritdoc />
    public bool IsNull => this._valueType is null && this._referenceType is null;

    /// <inheritdoc />
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


    /// <summary>
    /// Defines implicit conversion operators for the <see cref="Union{T1, T2, T3, T4, T5}"/> structure,
    /// allowing objects of the specified generic types to be automatically converted into a union instance.
    /// </summary>
    /// <param name="v">The value of the specific type to be wrapped in a union.</param>
    /// <returns>A new instance of <see cref="Union{T1, T2, T3, T4, T5}"/>.</returns>
    public static implicit operator Union<T1, T2, T3, T4, T5>(T1? v) => new(v);

    /// <summary>
    /// Provides an implicit conversion from the second possible union type to the union itself.
    /// </summary>
    /// <param name="v">The value of type T2 to be converted into the union.</param>
    /// <returns>A new instance of <c>Union&lt;T1, T2, T3, T4, T5&gt;</c> containing the provided value.</returns>
    public static implicit operator Union<T1, T2, T3, T4, T5>(T2? v) => new(v);

    /// <summary>
    /// Defines an implicit conversion operator for creating a union instance that holds a value of the third predefined type in the union.
    /// </summary>
    /// <param name="v">The value of the third type to be wrapped in the union.</param>
    /// <returns>A union instance containing the specified value.</returns>
    public static implicit operator Union<T1, T2, T3, T4, T5>(T3? v) => new(v);

    /// <summary>
    /// Defines an implicit conversion from the specified type to the union type.
    /// </summary>
    /// <param name="v">The value to convert and store in the union.</param>
    /// <typeparam name="T4">The fourth possible type of the union.</typeparam>
    public static implicit operator Union<T1, T2, T3, T4, T5>(T4? v) => new(v);

    /// <summary>
    /// Defines an implicit cast operator that allows a value of the fifth possible type
    /// to be converted into an instance of the union type.
    /// </summary>
    /// <param name="v">The value of the fifth type to be contained within the union.</param>
    /// <returns>An instance of the union containing the provided value as the fifth possible type.</returns>
    public static implicit operator Union<T1, T2, T3, T4, T5>(T5? v) => new(v);

    /// <summary>
    /// Returns a string that represents the current union value. If the value is null, it returns the string "null".
    /// </summary>
    /// <returns>
    /// A string representation of the current union value or "null" if the value is null.
    /// </returns>
    public override string ToString() => this.Value?.ToString() ?? "null";
}