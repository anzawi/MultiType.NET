namespace MultiType.NET.Core.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// A JSON converter for serializing and deserializing union types with five possible values, represented by
/// <see cref="Unions.Union{T1, T2, T3, T4, T5}"/>.
/// </summary>
/// <typeparam name="T1">The type of the first possible value in the union.</typeparam>
/// <typeparam name="T2">The type of the second possible value in the union.</typeparam>
/// <typeparam name="T3">The type of the third possible value in the union.</typeparam>
/// <typeparam name="T4">The type of the fourth possible value in the union.</typeparam>
/// <typeparam name="T5">The type of the fifth possible value in the union.</typeparam>
/// <remarks>
/// The JSON representation includes a discriminator property and a value property. The discriminator indicates which
/// type in the union is currently used, while the value property contains the actual serialized value. Both properties
/// are mandatory during serialization and deserialization.
/// </remarks>
public class UnionJsonConverter<T1, T2, T3, T4, T5> : JsonConverter<Unions.Union<T1, T2, T3, T4, T5>>
{
    /// <summary>
    /// A constant string used as a type discriminator to identify the type of value within a union during
    /// JSON serialization and deserialization processes.
    /// </summary>
    private const string TypeDiscriminator = "type";

    /// <summary>
    /// Defines the property name used to store the value within serialized JSON objects
    /// for Union types handled by the <see cref="UnionJsonConverter{T1, T2, T3, T4, T5}"/>.
    /// This property is essential for ensuring that the deserialized format correctly
    /// maps to the appropriate type in the union.
    /// </summary>
    private const string ValueProperty = "value";

    /// Reads and converts the JSON representation of a union type object into a strongly-typed Union instance.
    /// Throws a JsonException if the input JSON does not match the expected structure.
    /// <param name="reader">The Utf8JsonReader positioned at the start of the JSON object to convert.</param>
    /// <param name="typeToConvert">The type of object to convert (Union type).</param>
    /// <param name="options">The serialization options to use when deserializing the JSON.</param>
    /// <return>A Union instance containing the deserialized value from the JSON object.</return>
    public override Unions.Union<T1, T2, T3, T4, T5> Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("JSON object expected");

        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;

        if (!root.TryGetProperty(TypeDiscriminator, out var typeProperty))
            throw new JsonException($"Missing '{TypeDiscriminator}' discriminator");

        if (typeProperty.ValueKind != JsonValueKind.Number)
            throw new JsonException($"'{TypeDiscriminator}' must be a number");

        var typeIndex = typeProperty.GetInt32();

        if (!root.TryGetProperty(ValueProperty, out var valueElement))
            throw new JsonException($"Missing '{ValueProperty}' property");

        var valueJson = valueElement.GetRawText();

        return typeIndex switch
        {
            1 => JsonSerializer.Deserialize<T1>(valueJson, options)!,
            2 => JsonSerializer.Deserialize<T2>(valueJson, options)!,
            3 => JsonSerializer.Deserialize<T3>(valueJson, options)!,
            4 => JsonSerializer.Deserialize<T4>(valueJson, options)!,
            5 => JsonSerializer.Deserialize<T5>(valueJson, options)!,
            _ => throw new JsonException($"Invalid type discriminator: {typeIndex}"),
        };
    }

    /// <summary>
    /// Writes a JSON representation of a union value by serializing the type discriminator and the value.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used to write JSON.</param>
    /// <param name="value">The union value to be serialized to JSON.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> used for JSON serialization.</param>
    public override void Write(Utf8JsonWriter writer, Unions.Union<T1, T2, T3, T4, T5> value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        value.Match(
            _ => writer.WriteNumber(TypeDiscriminator, 1),
            _ => writer.WriteNumber(TypeDiscriminator, 2),
            _ => writer.WriteNumber(TypeDiscriminator, 3),
            _ => writer.WriteNumber(TypeDiscriminator, 4),
            _ => writer.WriteNumber(TypeDiscriminator, 5));

        writer.WritePropertyName(ValueProperty);
        JsonSerializer.Serialize(writer, value.Value, value.Type, options);

        writer.WriteEndObject();
    }
}