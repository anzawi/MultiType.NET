namespace MultiType.NET.Core.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// A JSON converter for serializing and deserializing objects into a union type with up to seven possible types.
/// This class supports discriminating between different types using a type discriminator stored in JSON.
/// </summary>
/// <typeparam name="T1">The first possible type for the union.</typeparam>
/// <typeparam name="T2">The second possible type for the union.</typeparam>
/// <typeparam name="T3">The third possible type for the union.</typeparam>
/// <typeparam name="T4">The fourth possible type for the union.</typeparam>
/// <typeparam name="T5">The fifth possible type for the union.</typeparam>
/// <typeparam name="T6">The sixth possible type for the union.</typeparam>
/// <typeparam name="T7">The seventh possible type for the union.</typeparam>
public class UnionJsonConverter<T1, T2, T3, T4, T5, T6, T7> : JsonConverter<Unions.Union<T1, T2, T3, T4, T5, T6, T7>>
{
    /// <summary>
    /// Represents the name of the JSON property used as a type discriminator in
    /// the serialization and deserialization process of a union type.
    /// </summary>
    /// <remarks>
    /// The type discriminator is a key property included in the JSON structure during
    /// serialization to indicate which type is being serialized. During deserialization,
    /// its value is used to determine the appropriate type to deserialize.
    /// </remarks>
    private const string TypeDiscriminator = "type";

    /// <summary>
    /// Represents the key name for the value property used within the JSON serialization
    /// process for a union type. This constant is leveraged to identify and handle the
    /// serialized value payload in conjunction with the type discriminator during
    /// deserialization and serialization.
    /// </summary>
    private const string ValueProperty = "value";

    /// Reads a JSON representation of a union type and deserializes it into the appropriate type.
    /// <param name="reader">The UTF-8 JSON reader to read data from.</param>
    /// <param name="typeToConvert">The type of the union to convert to.</param>
    /// <param name="options">The serializer options to use during deserialization.</param>
    /// <returns>A deserialized union object representing one of the seven generic types.</returns>
    public override Unions.Union<T1, T2, T3, T4, T5, T6, T7> Read(ref Utf8JsonReader reader, Type typeToConvert,
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
            6 => JsonSerializer.Deserialize<T6>(valueJson, options)!,
            7 => JsonSerializer.Deserialize<T7>(valueJson, options)!,
            _ => throw new JsonException($"Invalid type discriminator: {typeIndex}"),
        };
    }

    /// Writes a JSON representation of the specified union value to the provided
    /// JSON writer, including a type discriminator and the serialized value.
    /// <param name="writer">The JSON writer to write to.</param>
    /// <param name="value">The union value to serialize.</param>
    /// <param name="options">Serialization options to use during writing.</param>
    public override void Write(Utf8JsonWriter writer, Unions.Union<T1, T2, T3, T4, T5, T6, T7> value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        value.Match(
            _ => writer.WriteNumber(TypeDiscriminator, 1),
            _ => writer.WriteNumber(TypeDiscriminator, 2),
            _ => writer.WriteNumber(TypeDiscriminator, 3),
            _ => writer.WriteNumber(TypeDiscriminator, 4),
            _ => writer.WriteNumber(TypeDiscriminator, 5),
            _ => writer.WriteNumber(TypeDiscriminator, 6),
            _ => writer.WriteNumber(TypeDiscriminator, 7));

        writer.WritePropertyName(ValueProperty);
        JsonSerializer.Serialize(writer, value.Value, value.Type, options);

        writer.WriteEndObject();
    }
}