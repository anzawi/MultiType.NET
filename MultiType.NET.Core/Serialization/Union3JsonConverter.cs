namespace MultiType.NET.Core.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;
using Extensions;

/// <summary>
/// A JSON converter for serializing and deserializing instances of the <see cref="Unions.Union{T1, T2, T3}"/> class.
/// This converter handles the serialization of unions with a discriminated type format and the associated value.
/// </summary>
/// <typeparam name="T1">The type of the first possible value in the union.</typeparam>
/// <typeparam name="T2">The type of the second possible value in the union.</typeparam>
/// <typeparam name="T3">The type of the third possible value in the union.</typeparam>
public class UnionJsonConverter<T1, T2, T3> : JsonConverter<Unions.Union<T1, T2, T3>>
{
    /// <summary>
    /// Represents the name of the property used as a type discriminator in JSON serialization.
    /// </summary>
    private const string TypeDiscriminator = "type";

    /// <summary>
    /// Represents the property name "value" used for JSON serialization and deserialization of union types.
    /// This constant is utilized in JSON converters to correctly map the union's value to its serialized representation.
    /// </summary>
    private const string ValueProperty = "value";

    /// Reads and deserializes a JSON object into an instance of the Union{1, T2, T3} class.
    /// <param name="reader">The Utf8JsonReader from which to read the JSON data.</param>
    /// <param name="typeToConvert">The type of the object to convert. This parameter is required but not used in this method.</param>
    /// <param name="options">Options to configure the behavior of the JsonSerializer during reading.</param>
    /// <returns>A deserialized instance of Union{T1, T2, T3>} based on the JSON input.</returns>
    /// <exception cref="JsonException">
    /// Thrown when the input JSON is not a valid object, or the required "type" or "value" properties
    /// are missing, or the "type" property is not a number, or the "type" discriminator has
    /// an invalid value.
    /// </exception>
    public override Unions.Union<T1, T2, T3> Read(ref Utf8JsonReader reader, Type typeToConvert,
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
            _ => throw new JsonException($"Invalid type discriminator: {typeIndex}"),
        };
    }

    /// <summary>
    /// Serializes the given <see cref="Unions.Union{T1, T2, T3}"/> value to JSON.
    /// </summary>
    /// <param name="writer">
    /// The <see cref="Utf8JsonWriter"/> to write the JSON data to.
    /// </param>
    /// <param name="value">
    /// The <see cref="Unions.Union{T1, T2, T3}"/> instance to serialize.
    /// </param>
    /// <param name="options">
    /// Options to control the behavior of the <see cref="JsonSerializer"/> during writing.
    /// </param>
    public override void Write(Utf8JsonWriter writer, Unions.Union<T1, T2, T3> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        value.Match(
            _ => writer.WriteNumber(TypeDiscriminator, 1),
            _ => writer.WriteNumber(TypeDiscriminator, 2),
            _ => writer.WriteNumber(TypeDiscriminator, 3));

        writer.WritePropertyName(ValueProperty);
        JsonSerializer.Serialize(writer, value.Value, value.Type, options);

        writer.WriteEndObject();
    }
}