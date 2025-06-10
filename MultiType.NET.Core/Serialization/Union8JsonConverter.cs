namespace MultiType.NET.Core.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;
using Extensions;

/// <summary>
/// A custom JSON converter for serializing and deserializing instances of the <see cref="Unions.Union{T1, T2, T3, T4, T5, T6, T7, T8}"/> type.
/// This converter handles polymorphic serialization and deserialization by using a type discriminator and value property.
/// </summary>
/// <typeparam name="T1">The type of the first option in the union.</typeparam>
/// <typeparam name="T2">The type of the second option in the union.</typeparam>
/// <typeparam name="T3">The type of the third option in the union.</typeparam>
/// <typeparam name="T4">The type of the fourth option in the union.</typeparam>
/// <typeparam name="T5">The type of the fifth option in the union.</typeparam>
/// <typeparam name="T6">The type of the sixth option in the union.</typeparam>
/// <typeparam name="T7">The type of the seventh option in the union.</typeparam>
/// <typeparam name="T8">The type of the eighth option in the union.</typeparam>
public class
    UnionJsonConverter<T1, T2, T3, T4, T5, T6, T7, T8> : JsonConverter<Unions.Union<T1, T2, T3, T4, T5, T6, T7, T8>>
{
    /// <summary>
    /// Represents the name of the property in a serialized JSON object that is used as
    /// a discriminator to identify the specific type within a union.
    /// </summary>
    private const string TypeDiscriminator = "type";

    /// <summary>
    /// Represents the name of the property used to access the value within a JSON object
    /// when deserializing or serializing a union type.
    /// </summary>
    private const string ValueProperty = "value";

    /// Reads and converts the JSON to a Union{T1, T2, T3, T4, T5, T6, T7, T8} object.
    /// <param name="reader">The reader to read JSON data from.</param>
    /// <param name="typeToConvert">The type of the object to convert to.</param>
    /// <param name="options">Options to use during deserialization.</param>
    /// <returns>A Union{T1, T2, T3, T4, T5, T6, T7, T8} object converted from the JSON.</returns>
    /// <exception cref="JsonException">Thrown if the JSON is invalid or necessary properties are missing.</exception>
    public override Unions.Union<T1, T2, T3, T4, T5, T6, T7, T8> Read(ref Utf8JsonReader reader, Type typeToConvert,
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
            8 => JsonSerializer.Deserialize<T8>(valueJson, options)!,
            _ => throw new JsonException($"Invalid type discriminator: {typeIndex}"),
        };
    }

    /// Writes the JSON representation of the specified Union{T1, T2, T3, T4, T5, T6, T7, T8} object.
    /// <param name="writer">
    /// The Utf8JsonWriter to write the JSON data.
    /// </param>
    /// <param name="value">
    /// The Union{T1, T2, T3, T4, T5, T6, T7, T8} object to write.
    /// </param>
    /// <param name="options">
    /// The JsonSerializerOptions to use for serialization.
    /// </param>
    public override void Write(Utf8JsonWriter writer, Unions.Union<T1, T2, T3, T4, T5, T6, T7, T8> value,
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
            _ => writer.WriteNumber(TypeDiscriminator, 7),
            _ => writer.WriteNumber(TypeDiscriminator, 8));

        writer.WritePropertyName(ValueProperty);
        JsonSerializer.Serialize(writer, value.Value, value.Type, options);

        writer.WriteEndObject();
    }
}