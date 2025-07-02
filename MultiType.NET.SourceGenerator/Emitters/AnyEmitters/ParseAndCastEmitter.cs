using System.Linq;
using System.Text;

namespace MultiType.NET.SourceGenerator.Emitters.AnyEmitters;

internal static class ParseAndCastEmitter
{
    private static string GenerateTypeParameters(int arity) =>
        string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"));

    public static void EmitParseAndCastMethods(StringBuilder sb, int arity)
    {
        var typeParams = GenerateTypeParameters(arity);

        sb.AppendLine($$"""
                        public static bool TryParse(string input, IFormatProvider? _, out global::MultiType.NET.Core.Anys.Generated.Any<{{typeParams}}> result)
                        {
                            try
                            {
                              {{TryParseImplementation(arity)}}
                            }
                            catch
                            {
                                // ignore
                            }

                            result = default;
                            return false;
                        }
                        """);

        sb.AppendLine("""
                      public static bool TryCast<T>(string? input, out T? value)
                      {
                          value = default;

                          if (typeof(T).IsPrimitiveType(input, out var parsed))
                          {
                              value = (T)parsed!;
                              return true;
                          }

                          try
                          {
                              bool needsQuotes = typeof(T) == typeof(string)
                                                 && (input?.StartsWith("\"") != true && input?.EndsWith("\"") != true);

                              string jsonInput = needsQuotes ? $"\"{input}\"" : input!;

                              value = JsonSerializer.Deserialize<T>(jsonInput);
                              return value is not null;
                          }
                          catch
                          {
                              return false;
                          }
                      }
                      """);
    }

    private static string TryParseImplementation(int arity)
    {
        return string.Join("\n", Enumerable.Range(1, arity).Select(i => $$"""
                                                                          if (TryCast<T{{i}}>(input, out var t{{i}}))
                                                                          {
                                                                              result = FromT{{i}}(t{{i}});
                                                                              return true;
                                                                          }
                                                                          """));
    }
}