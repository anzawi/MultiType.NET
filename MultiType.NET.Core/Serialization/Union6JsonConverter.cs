namespace MultiType.NET.Core.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;
using Extensions;

/// <summary>
/// A custom JSON converter for serializing and deserializing instances of
/// <see cref="Unions.Union{T1, T2, T3, T4, T5, T6}"/> objects.
/// </summary>
/// <typeparam name="T1">The type parameter for the first possible type within the union.</typeparam>
/// <typeparam name="T2">The type parameter for the second possible type within the union.</typeparam>
/// <typeparam name="T3">The type parameter for the third possible type within the union.</typeparam>
/// <typeparam name="T4">The type parameter for the fourth possible type within the union.</typeparam>
/// <typeparam name="T5">The type parameter for the fifth possible type within the union.</typeparam>
/// <typeparam name="T6">The type parameter for the sixth possible type within the union.</typeparam>
/// <remarks>
/// This converter handles deserialization by inspecting a type discriminator
/// and a value field to determine which type to parse the data into.
/// It relies on JSON properties to map the type and the value appropriately.
/// </remarks>
public class UnionJsonConverter<T1, T2, T3, T4, T5, T6> : JsonConverter<Unions.Union<T1, T2, T3, T4, T5, T6>>
{
    /// <summary>
    /// Represents the name of the property used as a type discriminator in JSON serialization and deserialization.
    /// The type discriminator determines which specific type within a union is being serialized or deserialized.
    /// </summary>
    private const string TypeDiscriminator = "type";

    /// <summary>
    /// Represents the property name used to store the value of the union in JSON serialization
    /// and deserialization processes.
    /// </summary>
    private const string ValueProperty = "value";

    /// Reads and converts JSON data to an instance of the Union{T1, T2, T3, T4, T5, T6} type.
    /// <param name="reader">The Utf8JsonReader to read JSON data from.</param>
    /// <param name="typeToConvert">The type of the object to convert to. This parameter is not directly used in this method.</param>
    /// <param name="options">The JsonSerializerOptions to use for reading the JSON.</param>
    /// <returns>An instance of Union{T1, T2, T3, T4, T5, T6} deserialized from the JSON data.</returns>
    public override Unions.Union<T1, T2, T3, T4, T5, T6> Read(ref Utf8JsonReader reader, Type typeToConvert,
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
            _ => throw new JsonException($"Invalid type discriminator: {typeIndex}"),
        };
    }

    /// <summary>
    /// Writes a <see cref="Unions.Union{T1, T2, T3, T4, T5, T6}"/> object as JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> used to write the JSON output.</param>
    /// <param name="value">The <see cref="Unions.Union{T1, T2, T3, T4, T5, T6}"/> instance to write as JSON.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> used to customize the JSON writing behavior.</param>
    public override void Write(Utf8JsonWriter writer, Unions.Union<T1, T2, T3, T4, T5, T6> value,
        JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        value.Match(
            _ => writer.WriteNumber(TypeDiscriminator, 1),
            _ => writer.WriteNumber(TypeDiscriminator, 2),
            _ => writer.WriteNumber(TypeDiscriminator, 3),
            _ => writer.WriteNumber(TypeDiscriminator, 4),
            _ => writer.WriteNumber(TypeDiscriminator, 5),
            _ => writer.WriteNumber(TypeDiscriminator, 6));

        writer.WritePropertyName(ValueProperty);
        JsonSerializer.Serialize(writer, value.Value, value.Type, options);

        writer.WriteEndObject();
    }
}