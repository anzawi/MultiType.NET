namespace MultiType.NET.Core.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// A custom JSON converter for serializing and deserializing instances of the generic Union class that supports four possible types.
/// </summary>
/// <typeparam name="T1">The first possible type of the union.</typeparam>
/// <typeparam name="T2">The second possible type of the union.</typeparam>
/// <typeparam name="T3">The third possible type of the union.</typeparam>
/// <typeparam name="T4">The fourth possible type of the union.</typeparam>
/// <remarks>
/// This converter expects a JSON object containing a discriminator property to identify the type, as well as a value property to store the serialized data.
/// It supports round-tripping data via JSON serialization and deserialization.
/// </remarks>
public class UnionJsonConverter<T1, T2, T3, T4> : JsonConverter<Unions.Union<T1, T2, T3, T4>>
{
    /// <summary>
    /// Represents the name of the property used as a discriminator in JSON serialization
    /// to determine the type of a union.
    /// </summary>
    private const string TypeDiscriminator = "type";

    /// <summary>
    /// Represents the name of the JSON property used to store the value
    /// of a union type when serializing or deserializing with the
    /// UnionJsonConverter. This property is expected in JSON objects
    /// processed by the converter and is used to locate the actual
    /// value associated with the identified type.
    /// </summary>
    private const string ValueProperty = "value";

    /// Reads and deserializes a JSON representation into an instance of Union{T1, T2, T3, T4}.
    /// <param name="reader">
    /// The Utf8JsonReader from which to read the JSON data.
    /// </param>
    /// <param name="typeToConvert">
    /// The type of the object to convert; typically not used in this implementation.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior during serialization and deserialization.
    /// </param>
    /// <returns>
    /// The deserialized Union{T1, T2, T3, T4} instance.
    /// </returns>
    /// <exception cref="JsonException">
    /// Thrown when the JSON does not conform to the expected format or contains invalid type information.
    /// </exception>
    public override Unions.Union<T1, T2, T3, T4> Read(ref Utf8JsonReader reader, Type typeToConvert,
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
            _ => throw new JsonException($"Invalid type discriminator: {typeIndex}"),
        };
    }

    /// <summary>
    /// Writes the provided Union value to the specified Utf8JsonWriter.
    /// </summary>
    /// <param name="writer">The Utf8JsonWriter to which the value is written.</param>
    /// <param name="value">The Union instance containing one of the provided generic types.</param>
    /// <param name="options">The JsonSerializerOptions to customize the JSON serialization behavior.</param>
    public override void Write(Utf8JsonWriter writer, Unions.Union<T1, T2, T3, T4> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        value.Match(
            _ => writer.WriteNumber(TypeDiscriminator, 1),
            _ => writer.WriteNumber(TypeDiscriminator, 2),
            _ => writer.WriteNumber(TypeDiscriminator, 3),
            _ => writer.WriteNumber(TypeDiscriminator, 4));

        writer.WritePropertyName(ValueProperty);
        JsonSerializer.Serialize(writer, value.Value, value.Type, options);

        writer.WriteEndObject();
    }
}