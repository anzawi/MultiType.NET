namespace MultiType.NET.SourceGenerator.Emitters.JsonConverterEmitters;

using System.Linq;
using System.Text;

internal static class WriteMethodEmitter
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
                        /// Writes the JSON representation of the specified Any{{{typeParams}}} object.
                        /// <param name="writer">
                        /// The Utf8JsonWriter to write the JSON data.
                        /// </param>
                        /// <param name="value">
                        /// The Any{{{typeParams}}} object to write.
                        /// </param>
                        /// <param name="options">
                        /// The JsonSerializerOptions to use for serialization.
                        /// </param>
                        """);
    }

    private static void EmitMethod(StringBuilder sb, int arity, string typeParams)
    {
        sb.AppendLine($$"""
                        public override void Write(Utf8JsonWriter writer, global::MultiType.NET.Core.Anys.Generated.Any<{{typeParams}}> value,
                            JsonSerializerOptions options)
                        {
                            writer.WriteStartObject();
                        
                            value.Match(
                                {{string.Join(", ", Enumerable.Range(1, arity).Select(i => $"_ => writer.WriteNumber(TypeDiscriminator, {i}) \n"))}}
                            );
                        
                            writer.WritePropertyName(ValueProperty);
                            JsonSerializer.Serialize(writer, value.Value, value.Type, options);
                        
                            writer.WriteEndObject();
                        }
                        """);
    }
}