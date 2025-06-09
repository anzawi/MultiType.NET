namespace MultiType.NET.Core.Serialization;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Provides a custom JSON converter for handling serialization and deserialization of union types
/// with two possible value types.
/// </summary>
/// <typeparam name="T1">The first possible type of the union value.</typeparam>
/// <typeparam name="T2">The second possible type of the union value.</typeparam>
/// <remarks>
/// The converter uses a type discriminator and a value property for JSON representation. The
/// type discriminator indicates which of the two types the value corresponds to.
/// </remarks>
public class UnionJsonConverter<T1, T2> : JsonConverter<Unions.Union<T1, T2>>
{
    /// <summary>
    /// A constant string value used as a type discriminator property in the JSON serialization
    /// and deserialization of union types. It serves to differentiate between multiple potential
    /// type values in a union by identifying the type associated with the serialized data.
    /// </summary>
    private const string TypeDiscriminator = "type";

    /// <summary>
    /// Represents a constant string used as the property key for the value element
    /// during JSON serialization and deserialization of a union type.
    /// </summary>
    /// <remarks>
    /// This property key is expected to be present in serialized JSON objects and
    /// is used to store or retrieve the value portion of union types.
    /// </remarks>
    private const string ValueProperty = "value";

    /// Reads and deserializes a JSON representation of a union type containing two possible types.
    /// <param name="reader">
    /// The UTF-8 JSON reader to read from.
    /// </param>
    /// <param name="typeToConvert">
    /// The type of the object to convert, relevant to the deserialization process.
    /// </param>
    /// <param name="options">
    /// The serializer options to customize the JSON reading and deserialization behavior.
    /// </param>
    /// <returns>
    /// A union type instance of either T1 or T2 based on the deserialized JSON data.
    /// </returns>
    /// <exception cref="JsonException">
    /// Thrown if the provided JSON does not follow the expected format or contains invalid data.
    /// </exception>
    public override Unions.Union<T1, T2> Read(ref Utf8JsonReader reader, Type typeToConvert,
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
            _ => throw new JsonException($"Invalid type discriminator: {typeIndex}"),
        };
    }

    /// <summary>
    /// Serializes a <see cref="Unions.Union{T1, T2}"/> object to JSON.
    /// </summary>
    /// <param name="writer">The <see cref="Utf8JsonWriter"/> to which the JSON will be written.</param>
    /// <param name="value">The <see cref="Unions.Union{T1, T2}"/> object to be serialized.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> used for serialization.</param>
    public override void Write(Utf8JsonWriter writer, Unions.Union<T1, T2> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        value.Match(
            _ => writer.WriteNumber(TypeDiscriminator, 1),
            _ => writer.WriteNumber(TypeDiscriminator, 2));

        writer.WritePropertyName(ValueProperty);
        JsonSerializer.Serialize(writer, value.Value, value.Type, options);

        writer.WriteEndObject();
    }
}