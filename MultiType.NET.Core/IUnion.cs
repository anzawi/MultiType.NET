namespace MultiType.NET.Core;

/// <summary>
/// Represents a union type that can hold a value of one of several predefined types.
/// </summary>
public interface IUnion
{
    /// <summary>
    /// Gets the internal numeric index representing the type of the current value held by the union.
    /// </summary>
    /// <remarks>
    /// The <c>TypeIndex</c> property is used to determine which type from the defined set of possible types
    /// is currently represented by the union. This value is typically used internally or in utility methods
    /// to facilitate operations such as matching or type-checking.
    /// </remarks>
    byte TypeIndex { get; }

    /// <summary>
    /// Gets the value stored within the union. This value can be of any type that was defined in the union.
    /// The property returns null if the union is empty or holds a null value.
    /// </summary>
    object? Value { get; }

    /// Gets the runtime type of the value currently held by the union.
    /// If the union does not hold any value, the type returned is `typeof(void)`.
    Type Type { get; }

    /// <summary>
    /// Determines whether the current union value is of the specified type.
    /// </summary>
    /// <typeparam name="T">The type to check against the current value.</typeparam>
    /// <returns>True if the current value is of the specified type; otherwise, false.</returns>
    bool Is<T>();

    /// <summary>
    /// Retrieves the value contained in the union and casts it to the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The target type to cast the union's value to.</typeparam>
    /// <returns>
    /// The value of the union cast to type <typeparamref name="T"/> if the contained value is of that type.
    /// </returns>
    /// <exception cref="InvalidCastException">
    /// Thrown if the value cannot be cast to the specified type <typeparamref name="T"/>.
    /// </exception>
    T As<T>();

    /// <summary>
    /// Gets a value indicating whether the union holds a null value.
    /// </summary>
    /// <remarks>
    /// This property evaluates to true if the underlying value of the union is null or
    /// if there is no value assigned to the union. It accounts for both reference-type
    /// and value-type scenarios.
    /// </remarks>
    bool IsNull { get; }

    /// <summary>
    /// Attempts to cast the current value of the union into the specified reference type.
    /// If the union holds a null value, the method returns null.
    /// </summary>
    /// <typeparam name="T">The reference type to which the current value should be cast.</typeparam>
    /// <returns>
    /// The value of the union cast to the specified reference type, or null if the value is null.
    /// </returns>
    T? AsNullable<T>() where T : class;

    /// <summary>
    /// Converts the current value to a nullable struct of the specified type
    /// if the underlying value is not null.
    /// </summary>
    /// <typeparam name="T">
    /// The struct type to which the value should be converted.
    /// </typeparam>
    /// <returns>
    /// A nullable representation of the underlying struct value if it
    /// is not null; otherwise, null.
    /// </returns>
    T? AsNullableStruct<T>() where T : struct;
}