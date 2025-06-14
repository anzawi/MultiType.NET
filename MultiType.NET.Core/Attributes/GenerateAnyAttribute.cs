namespace MultiType.NET.Core.Attributes;

/// <summary>
/// Specifies that the target structure should act as a `Any` type and defines the types
/// that can be part of the `Any`.
/// ---
/// Applying this attribute to a struct allows it to represent multiple types by
/// specifying the allowable types as parameters.
/// </summary>
/// <remarks>
/// <b>⚠️ **IMPORTANT:**</b> You must install both packages:
/// - `MultiType.NET.Core`
/// - `MultiType.NET.Generators`
/// Without the generator, `[GenerateAny]` will have no effect and no types will be generated.
/// </remarks>
[AttributeUsage(AttributeTargets.Struct)]
public sealed class GenerateAnyAttribute(params Type[] types) : Attribute
{
    /// <summary>
    /// Gets the array of types designated for use in the union.
    /// This property holds the collection of types that are allowed to be part of the union when the attribute is applied.
    /// </summary>
    public Type[] Types { get; } = types ?? throw new ArgumentNullException(nameof(types));
}