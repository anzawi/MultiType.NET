namespace MultiType.NET.SourceGenerator.Emitters.JsonConverterEmitters;

using System.Linq;
using System.Text;

internal static class HeadersEmitter
{
    private static string GenerateTypeParameters(int arity) =>
        string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"));

    public static void EmitHeaders(StringBuilder sb, int arity, string unionNamespace, string jsonConverterNamespace)
    {
        var typeParams = GenerateTypeParameters(arity);
        EmitClassHeaders(sb, unionNamespace, jsonConverterNamespace);
        EmitClassDocs(sb, arity, typeParams, unionNamespace);
        EmitClass(sb, typeParams);
    }

    private static void EmitClassHeaders(StringBuilder sb, string unionNamespace, string jsonConverterNamespace)
    {
        sb.AppendLine($"""
                       namespace {jsonConverterNamespace};
                       using System.Text.Json;
                       using System.Text.Json.Serialization;
                       using Extensions;
                       using {unionNamespace};
                       """
        );
    }

    private static void EmitClassDocs(StringBuilder sb, int arity, string typeParams, string unionNamespace)
    {
        sb.AppendLine($$"""
                        /// <summary>
                        /// A custom JSON converter for serializing and deserializing instances of the <see cref="{{unionNamespace}}.Union{{{typeParams}}}"/> type.
                        /// This converter handles polymorphic serialization and deserialization by using a type discriminator and value property.
                        /// </summary>
                        """);
        for (int i = 0; i < arity; i++)
        {
            sb.AppendLine($"""/// <typeparam name="T{i+1}">The type of the {i+1} option in the union.</typeparam>""");
        }
    }

    private static void EmitClass(StringBuilder sb, string typeParams)
    {
        sb.AppendLine($$"""
                        public class
                            UnionJsonConverter<{{typeParams}}> : JsonConverter<Union<{{typeParams}}>>
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
                        """
        );
    }
}