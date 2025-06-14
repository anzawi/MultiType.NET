namespace MultiType.NET.SourceGenerator.Emitters.JsonConverterEmitters.ConvertorFactoryEmitters;

using System.Linq;
using System.Text;

internal static class FactoryEmitter
{
    public static void EmitFactory(StringBuilder sb, int arity)
    {
        sb.AppendLine($$"""
                        namespace MultiType.NET.Core.Serialization.Generated;
                        using System.Text.Json;
                        using System.Text.Json.Serialization;
                        /// <summary>
                        /// Provides a factory implementation for creating JSON converters tailored to handle Any types.
                        /// </summary>
                        /// <remarks>
                        /// This factory dynamically generates a specific JSON converter for a Any type based on the generic
                        /// arguments provided. The converter supports Any types with up to 8 generic type parameters.
                        /// </remarks>
                        public class AnyJsonConverterFactory : JsonConverterFactory
                        {
                            /// Determines whether the specified type can be converted by this JsonConverterFactory.
                            /// <param name="typeToConvert">The type to check for convertibility.</param>
                            /// <returns>
                            /// True if the type can be converted; otherwise, false.
                            /// </returns>
                            public override bool CanConvert(Type typeToConvert)
                            {
                                if (!typeToConvert.IsValueType || !typeToConvert.IsGenericType)
                                    return false;

                                var genericType = typeToConvert.GetGenericTypeDefinition();
                                return genericType.FullName!.StartsWith("MultiType.NET.Core.Anys.Generated.Any");
                            }

                            /// <summary>
                            /// Creates a JsonConverter instance for the specified type.
                            /// </summary>
                            /// <param name="typeToConvert">The type of object to create a converter for.</param>
                            /// <param name="options">The serialization options to use when creating the converter.</param>
                            /// <returns>A JsonConverter instance capable of handling the specified type.</returns>
                            public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
                            {
                                var args = typeToConvert.GetGenericArguments();
                                return args.Length switch
                                {
                                    {{string.Join(",\n ", Enumerable.Range(2, arity-1)
                                        .Select(i => $"{i} => (JsonConverter)Activator.CreateInstance(typeof(AnyJsonConverter<{RepeatComma(i)}>).MakeGenericType(args))!"))}},
                                    _ => throw new NotSupportedException($"Any<{args.Length}> is not supported. Try 'MuliType.NET.SourceGenerator' to generate +17 types with its convertors."),
                                };
                            }
                        }
                        """);
    }

    private static string RepeatComma(int numberOfCommas)
    {
        return new string(',', numberOfCommas - 1);
    }
}