namespace MultiType.NET.Core.Unions;

/// <inheritdoc />
public readonly struct Union<T1, T2, T3> : IUnion
{
    private readonly ValueType? _valueType;
    private readonly object? _referenceType;
    private readonly bool _isValueType;

    /// <inheritdoc />
    public byte TypeIndex { get; }

    /// <summary>
    /// Represents a discriminated union type that can hold a value of one of three predefined types.
    /// </summary>
    /// <typeparam name="T1">The first type that the union can represent.</typeparam>
    /// <typeparam name="T2">The second type that the union can represent.</typeparam>
    /// <typeparam name="T3">The third type that the union can represent.</typeparam>
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
    /// Represents a union type that can hold a value of one of three predefined types.
    /// </summary>
    /// <typeparam name="T1">The first possible type of the union.</typeparam>
    /// <typeparam name="T2">The second possible type of the union.</typeparam>
    /// <typeparam name="T3">The third possible type of the union.</typeparam>
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
    /// Represents a union type that can hold a value of one of three predefined types.
    /// This structure encapsulates a value of type T1, T2, or T3 and provides functionality to
    /// safely retrieve and inspect the underlying value.
    /// </summary>
    /// <typeparam name="T1">The first possible type of the union.</typeparam>
    /// <typeparam name="T2">The second possible type of the union.</typeparam>
    /// <typeparam name="T3">The third possible type of the union.</typeparam>
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


    /// <summary>
    /// Defines implicit conversion operators for creating a union instance from types T1, T2, or T3.
    /// </summary>
    /// <param name="v">The value of type T1 to be converted into the union.</param>
    /// <returns>An instance of the union containing the specified value of type T1.</returns>
    public static implicit operator Union<T1, T2, T3>(T1? v) => new(v);

    /// <summary>
    /// Defines an implicit conversion from a value of the second type to a union type that represents one of three predefined types.
    /// </summary>
    /// <typeparam name="T1">The first type that the union can represent.</typeparam>
    /// <typeparam name="T2">The second type that the union can represent.</typeparam>
    /// <typeparam name="T3">The third type that the union can represent.</typeparam>
    /// <param name="v">The value of the second type to convert into the union.</param>
    /// <returns>A union instance containing the provided value.</returns>
    public static implicit operator Union<T1, T2, T3>(T2? v) => new(v);

    /// <summary>
    /// Defines an implicit conversion from the specified type T3 to an instance of Union.
    /// </summary>
    /// <typeparam name="T1">The first possible type that the union can represent.</typeparam>
    /// <typeparam name="T2">The second possible type that the union can represent.</typeparam>
    /// <typeparam name="T3">The third possible type that the union can represent.</typeparam>
    /// <param name="v">The value of type T3 to be converted into a union.</param>
    /// <returns>An instance of Union containing the provided value of type T3.</returns>
    public static implicit operator Union<T1, T2, T3>(T3? v) => new(v);

    /// <summary>
    /// Returns a string representation of the current union value.
    /// </summary>
    /// <returns>
    /// A string that represents the union's current value. If the value is null, "null" is returned.
    /// </returns>
    public override string ToString() => this.Value?.ToString() ?? "null";
}