namespace MultiType.NET.SourceGenerator.Emitters.JsonConverterEmitters;

using System.Linq;
using System.Text;

internal static class ReadMethodEmitter
{
    private static string GenerateTypeParameters(int arity) =>
        string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"));

    public static void EmitReadMethod(StringBuilder sb, int arity)
    {
        var typeParams = GenerateTypeParameters(arity);
        EmitClassDocs(sb, typeParams);
        EmitMethod(sb, arity, typeParams);
    }

    private static void EmitClassDocs(StringBuilder sb, string typeParams)
    {
        sb.AppendLine($$"""
                        // Reads and converts the JSON to a Union{{{typeParams}}} object.
                        /// <param name="reader">The reader to read JSON data from.</param>
                        /// <param name="typeToConvert">The type of the object to convert to.</param>
                        /// <param name="options">Options to use during deserialization.</param>
                        /// <returns>A Union{{{typeParams}}} object converted from the JSON.</returns>
                        /// <exception cref="JsonException">Thrown if the JSON is invalid or necessary properties are missing.</exception>
                        """);
    }

    private static void EmitMethod(StringBuilder sb, int arity, string typeParams)
    {
        sb.AppendLine($$"""
                        public override Union<{{typeParams}}> Read(ref Utf8JsonReader reader, Type typeToConvert,
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
                            {{string.Join(", ", Enumerable.Range(1, arity).Select(i => $"{i} => JsonSerializer.Deserialize<T{i}>(valueJson, options)! \n"))}},
                                _ => throw new JsonException($"Invalid type discriminator: {typeIndex}"),
                            };
                        }
                        """);
    }
}