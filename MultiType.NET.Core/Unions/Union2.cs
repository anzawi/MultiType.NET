namespace MultiType.NET.Core.Unions;

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