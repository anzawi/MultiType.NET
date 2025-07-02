namespace MultiType.NET.SourceGenerator.Emitters.JsonConverterEmitters;

using System.Linq;
using System.Text;

internal static class HeadersEmitter
{
    private static string GenerateTypeParameters(int arity) =>
        string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"));

    public static void EmitHeaders(StringBuilder sb, int arity, string anyNamespace, string jsonConverterNamespace)
    {
        var typeParams = GenerateTypeParameters(arity);
        EmitClassHeaders(sb, anyNamespace, jsonConverterNamespace);
        EmitClassDocs(sb, arity, typeParams, anyNamespace);
        EmitClass(sb, typeParams);
    }

    private static void EmitClassHeaders(StringBuilder sb, string anyNamespace, string jsonConverterNamespace)
    {
        sb.AppendLine($"""
                       namespace {jsonConverterNamespace};
                       using System.Text.Json;
                       using System.Text.Json.Serialization;
                       using {anyNamespace};
                       """
        );
    }

    private static void EmitClassDocs(StringBuilder sb, int arity, string typeParams, string anyNamespace)
    {
        sb.AppendLine($$"""
                        /// <summary>
                        /// A custom JSON converter for serializing and deserializing instances of the <see cref="{{anyNamespace}}.Any{{{typeParams}}}"/> type.
                        /// This converter handles polymorphic serialization and deserialization by using a type discriminator and value property.
                        /// </summary>
                        """);
        for (var i = 0; i < arity; i++)
        {
            sb.AppendLine($"""/// <typeparam name="T{i+1}">The type of the {i+1} option in the Any.</typeparam>""");
        }
    }

    private static void EmitClass(StringBuilder sb, string typeParams)
    {
        sb.AppendLine($$"""
                        public class
                            AnyJsonConverter<{{typeParams}}> : JsonConverter<global::MultiType.NET.Core.Anys.Generated.Any<{{typeParams}}>>
                        {
                            /// <summary>
                            /// Represents the name of the property in a serialized JSON object that is used as
                            /// a discriminator to identify the specific type within a Any.
                            /// </summary>
                            private const string TypeDiscriminator = "type";

                            /// <summary>
                            /// Represents the name of the property used to access the value within a JSON object
                            /// when deserializing or serializing a Any type.
                            /// </summary>
                            private const string ValueProperty = "value";
                        """
        );
    }
}