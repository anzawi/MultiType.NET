namespace MultiType.NET.SourceGenerator.Emitters.UnionEmitters;

using System.Linq;
using System.Text;

public static class BaseMethodsEmitter
{
    private const string NullString = "null";

    private static string GenerateTypeParameters(int arity) =>
        string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"));

    public static void EmitBaseMethods(StringBuilder sb, int arity, string unionNamespace)
    {
        var typeParams = GenerateTypeParameters(arity);
        EmitUnionClassHeader(sb, typeParams, unionNamespace);
        EmitUnionFields(sb);
        EmitUnionProperties(sb);
        EmitAllowedTypes(sb, arity);
        EmitUnionMethods(sb, arity);
        EmitConstructors(sb, arity, typeParams);
        EmitImplicitOperators(sb, arity, typeParams);
        EmitToString(sb);
    }

    private static void EmitUnionClassHeader(StringBuilder sb, string typeParams, string unionNamespace) =>
        sb.AppendLine($$"""
                            namespace {{unionNamespace}};
                            using Exceptions;
                            using Helpers;
                            using System.Diagnostics;
                            using System.Runtime.CompilerServices;
                            
                            /// <inheritdoc />
                            [DebuggerDisplay("{DebuggerDisplay,nq}")]
                            public readonly struct Union<{{typeParams}}> : IUnion
                            {
                            private string DebuggerDisplay =>
                                TypeIndex == 0
                                    ? "[Uninitialized]"
                                    : $"[TypeIndex: {TypeIndex}] Value = {Value} ({Value?.GetType().Name})";
                        """);

    private static void EmitUnionFields(StringBuilder sb) =>
        sb.AppendLine("""
                          private readonly ValueType? _valueType;
                          private readonly object? _referenceType;
                          private readonly bool _isValueType;
                      """);

    private static void EmitAllowedTypes(StringBuilder sb, int arity)
    {
        var typeArray = string.Join(", ",
            Enumerable.Range(1, arity).Select(i => $"typeof(T{i})"));

        sb.AppendLine($$"""
                        /// <summary>
                        /// The set of allowed types for this union.
                        /// </summary>
                        public static Type[] AllowedTypes => new[] { {{typeArray}} };

                        """);
    }

    private static void EmitUnionProperties(StringBuilder sb) =>
        sb.AppendLine("""
                          /// <inheritdoc />
                          public byte TypeIndex { get; }
                          
                          /// <inheritdoc />
                          public object? Value => _isValueType ? _valueType : _referenceType;
                          
                          /// <inheritdoc />
                          public Type Type => Value?.GetType() ?? typeof(void);
                          
                          /// <inheritdoc />
                          public bool IsNull => _valueType is null && _referenceType is null;
                      """);

    private static void EmitUnionMethods(StringBuilder sb, int arity)
    {
        var typeCheckConditions = string.Join(" || ",
            Enumerable.Range(1, arity).Select(i => $"typeof(T) == typeof(T{i}) && TypeIndex == {i}"));

        sb.AppendLine($$"""
                            /// <inheritdoc />
                            [MethodImpl(MethodImplOptions.AggressiveInlining)]
                            public bool Is<T>() => Value is T;
                            
                            /// <inheritdoc />
                            [MethodImpl(MethodImplOptions.AggressiveInlining)]
                            public T As<T>()
                            {
                                if ({{typeCheckConditions}})
                                {
                                    return Value is T val
                                        ? val
                                        : throw new InvalidCastException(
                                            $"Cannot cast union value of type {Value?.GetType().Name ?? "{{NullString}}"} to {typeof(T).Name}");
                                }
                                throw new InvalidCastException($"Type {typeof(T).Name} is not one of the union type parameters");
                            }
                            
                            /// <inheritdoc />
                            [MethodImpl(MethodImplOptions.AggressiveInlining)]
                            public T? AsNullable<T>() where T : class
                            {
                                if (IsNull) return null;
                                return As<T>();
                            }
                            
                            /// <inheritdoc />
                            [MethodImpl(MethodImplOptions.AggressiveInlining)]
                            public T? AsNullableStruct<T>() where T : struct
                            {
                                if (IsNull) return null;
                                return As<T>();
                            }
                        """);
    }

    private static void EmitConstructors(StringBuilder sb, int arity, string typeParams)
    {
        for (int i = 1; i <= arity; i++)
        {
            sb.AppendLine($$"""
                                public Union(T{{i}}? value)
                                {
                                    TypeIndex = {{i}};
                                    if (value is null)
                                    {
                                        _valueType = null;
                                        _referenceType = null;
                                        _isValueType = false;
                                        return;
                                    }
                                    
                                    if (value is ValueType vt)
                                    {
                                        _valueType = vt;
                                        _referenceType = null;
                                        _isValueType = true;
                                    }
                                    else
                                    {
                                        _valueType = null;
                                        _referenceType = value;
                                        _isValueType = false;
                                    }
                                }
                            """);
        }
    }

    private static void EmitImplicitOperators(StringBuilder sb, int arity, string typeParams)
    {
        for (int i = 1; i <= arity; i++)
        {
            sb.AppendLine($$"""
                                public static implicit operator Union<{{typeParams}}>(T{{i}}? v) => new(v);
                            """);
        }
    }

    private static void EmitToString(StringBuilder sb) =>
        sb.AppendLine("""
                      /// <summary>
                      // Returns a string representation of the current union value.
                      /// </summary>
                      public override string ToString() 
                      {
                          if (TypeIndex == 0)
                           return "Union[Uninitialized]";
                      
                          string typeName = Value?.GetType().Name ?? "null";
                          string valueStr = Value?.ToString() ?? "null";
                          
                          return $"Union[{typeName}] = {valueStr}";
                      }
                      """);
}