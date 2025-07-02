namespace MultiType.NET.SourceGenerator.Emitters.AnyEmitters;

using System.Linq;
using System.Text;

internal static class EqualityEmitter
{
    public static void EmitEqualityMembers(StringBuilder sb, int arity)
    {
        var typeParams = string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"));
        var anyType = $"global::MultiType.NET.Core.Anys.Generated.Any<{typeParams}>";

        sb.AppendLine($$"""
                            public override bool Equals(object? obj)
                            {
                                // Fast path: Reference equality
                                if (ReferenceEquals(this, obj)) return true;

                                // Check exact type (prevent cross-type comparison)
                                if (obj is not {{anyType}} other)
                                    return false;

                                // Fast path: Same index and both null
                                if (TypeIndex == other.TypeIndex && Value is null && other.Value is null)
                                    return true;

                                // Full equality check
                                return TypeIndex == other.TypeIndex &&
                                       Equals(Value, other.Value);
                            }

                            public override int GetHashCode()
                            {
                                // Recommended pattern: HashCode.Combine (C# 8+)
                                return HashCode.Combine(TypeIndex, Value);
                            }

                            public static bool operator ==({{anyType}} left, {{anyType}} right)
                                => left.Equals(right);

                            public static bool operator !=({{anyType}} left, {{anyType}} right)
                                => !left.Equals(right);
                        """);
    }
}