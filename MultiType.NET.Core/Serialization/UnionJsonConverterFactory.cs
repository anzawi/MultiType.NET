namespace MultiType.NET.Core.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Provides a factory implementation for creating JSON converters tailored to handle union types.
/// </summary>
/// <remarks>
/// This factory dynamically generates a specific JSON converter for a union type based on the generic
/// arguments provided. The converter supports union types with up to 8 generic type parameters.
/// </remarks>
public class UnionJsonConverterFactory : JsonConverterFactory
{
    /// Determines whether the specified type can be converted by this JsonConverterFactory.
    /// <param name="typeToConvert">The type to check for convertibility.</param>
    /// <returns>
    /// True if the type can be converted; otherwise, false.
    /// </returns>
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsValueType || !typeToConvert.IsGenericType)
            return false;

        var genericType = typeToConvert.GetGenericTypeDefinition();
        return genericType.FullName!.StartsWith("MultiType.NET.Core.Unions.Union");
    }

    /// <summary>
    /// Creates a JsonConverter instance for the specified type.
    /// </summary>
    /// <param name="typeToConvert">The type of object to create a converter for.</param>
    /// <param name="options">The serialization options to use when creating the converter.</param>
    /// <returns>A JsonConverter instance capable of handling the specified type.</returns>
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var args = typeToConvert.GetGenericArguments();
        return args.Length switch
        {
            2 => (JsonConverter)Activator.CreateInstance(typeof(UnionJsonConverter<,>).MakeGenericType(args))!,
            3 => (JsonConverter)Activator.CreateInstance(typeof(UnionJsonConverter<,,>).MakeGenericType(args))!,
            4 => (JsonConverter)Activator.CreateInstance(typeof(UnionJsonConverter<,,,>).MakeGenericType(args))!,
            5 => (JsonConverter)Activator.CreateInstance(typeof(UnionJsonConverter<,,,,>).MakeGenericType(args))!,
            6 => (JsonConverter)Activator.CreateInstance(typeof(UnionJsonConverter<,,,,,>).MakeGenericType(args))!,
            7 => (JsonConverter)Activator.CreateInstance(typeof(UnionJsonConverter<,,,,,,>).MakeGenericType(args))!,
            8 => (JsonConverter)Activator.CreateInstance(typeof(UnionJsonConverter<,,,,,,,>).MakeGenericType(args))!,
            _ => throw new NotSupportedException($"Union<{args.Length}> is not supported."),
        };
    }
}