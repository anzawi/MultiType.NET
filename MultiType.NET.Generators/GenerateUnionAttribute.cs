namespace MultiType.NET.Generators;

/// <summary>
/// Specifies that the target structure or class should act as a union type and defines the types
/// that can be part of the union.
/// </summary>
/// <remarks>
/// Applying this attribute to a struct or class allows it to represent multiple types by
/// specifying the allowable types as parameters.
/// </remarks>
/// <param name="types">An array of <see cref="Type"/> objects representing the types that the union can include.</param>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class GenerateUnionAttribute(params Type[] types) : Attribute
{
    /// <summary>
    /// Gets the array of types designated for use in the union.
    /// This property holds the collection of types that are allowed to be part of the union when the attribute is applied.
    /// </summary>
    public Type[] Types { get; } = types;
}