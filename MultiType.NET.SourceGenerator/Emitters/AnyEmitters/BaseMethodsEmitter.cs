namespace MultiType.NET.SourceGenerator.Emitters.AnyEmitters;

using System;
using System.Linq;
using System.Text;

public static class BaseMethodsEmitter
{
    private const string NullString = "null";

    private static string GenerateTypeParameters(int arity) =>
        string.Join(", ", Enumerable.Range(1, arity).Select(i => $"T{i}"));

    public static void EmitBaseMethods(StringBuilder sb, int arity, string anyNamespace)
    {
        var typeParams = GenerateTypeParameters(arity);
        EmitAnyClassHeader(sb, typeParams, anyNamespace);
        EmitAnyFields(sb);
        EmitAnyProperties(sb);
        EmitAllowedTypes(sb, arity);
        EmitAnyMethods(sb, arity);
        EmitFromMethods(sb, arity, typeParams);
        EmitFromTnMethods(sb, arity, typeParams);
        EmitConstructors(sb, arity);
        EmitImplicitOperators(sb, arity, typeParams);
        EmitToString(sb);
    }

    private static void EmitAnyClassHeader(StringBuilder sb, string typeParams, string anyNamespace) =>
        sb.AppendLine($$"""
                            namespace {{anyNamespace}};
                            using Exceptions;
                            using Helpers;
                            using System.Diagnostics;
                            using System.Runtime.CompilerServices;
                            using System.Text.Json.Serialization;
                            using {{anyNamespace.Replace("Anys", "Serialization")}};
                            
                            /// <inheritdoc />
                            [JsonConverter(typeof(AnyJsonConverterFactory))]
                            [DebuggerDisplay("{DebuggerDisplay,nq}")]
                            public readonly struct Any<{{typeParams}}> : IAny
                            {
                            private string DebuggerDisplay =>
                                TypeIndex == 0
                                    ? "[Uninitialized]"
                                    : $"[TypeIndex: {TypeIndex}] Value = {Value} ({Value?.GetType().Name})";
                        """);

    private static void EmitAnyFields(StringBuilder sb) =>
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
                        /// The set of allowed types for this Any.
                        /// </summary>
                        public static Type[] AllowedTypes => new[] { {{typeArray}} };

                        """);
    }

    private static void EmitAnyProperties(StringBuilder sb) =>
        sb.AppendLine("""
                          /// <inheritdoc />
                          public byte TypeIndex { get; }
                          
                          /// <inheritdoc />
                          public object? Value => _isValueType ? _valueType : _referenceType;
                          
                          /// <inheritdoc />
                          public Type Type => Value?.GetType() ?? typeof(void);
                          
                          /// <inheritdoc />
                          public bool HasValue => this.Value is not null;
                          
                          /// <inheritdoc />
                          public bool IsNull => _valueType is null && _referenceType is null;
                      """);

    private static void EmitAnyMethods(StringBuilder sb, int arity)
    {
        var typeCheckConditions = string.Join(" || ",
            Enumerable.Range(1, arity).Select(i => $"typeof(T) == typeof(T{i}) && TypeIndex == {i}"));

        sb.AppendLine($$"""
                            /// <inheritdoc />
                            [MethodImpl(MethodImplOptions.AggressiveInlining)]
                            public bool Is<T>() => Value is T;
                            
                            /// <inheritdoc />
                            public T As<T>()
                            {
                                if ({{typeCheckConditions}})
                                {
                                    return Value is T val
                                        ? val
                                        : throw new InvalidCastException(
                                            $"Cannot cast Any value of type {Value?.GetType().Name ?? "{{NullString}}"} to {typeof(T).Name}");
                                }
                                throw new InvalidCastException($"Type {typeof(T).Name} is not one of the Any type parameters");
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

    private static void EmitConstructors(StringBuilder sb, int arity)
    {
        for (var i = 1; i <= arity; i++)
        {
            sb.AppendLine($$"""
                                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                private Any(T{{i}}? value)
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
        for (var i = 1; i <= arity; i++)
        {
            sb.AppendLine($$"""
                                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                                public static implicit operator Any<{{typeParams}}>(T{{i}}? v) => new(v);
                            """);
        }
    }

    private static void EmitToString(StringBuilder sb) =>
        sb.AppendLine("""
                      /// <summary>
                      // Returns a string representation of the current Any value.
                      /// </summary>
                      public override string ToString() 
                      {
                          if (TypeIndex == 0)
                           return "Any[Uninitialized]";

                          string typeName = Value?.GetType().Name ?? "null";
                          string valueStr = Value?.ToString() ?? "null";
                          
                          return $"Any[{typeName}] = {valueStr}";
                      }
                      """);

    private static void EmitFromMethods(StringBuilder sb, int arity, string typeParams)
    {
        sb.AppendLine($$"""
                        /// <summary>
                        /// Creates a new Any from the given value.
                        /// </summary>
                        /// <remarks>
                        /// ⚠️ <b>Performance Warning:</b>
                        /// This method causes boxing of value types. For best performance, use <c>FromTn</c>, or implicit casting instead.
                        /// </remarks>
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public static Any<{{typeParams}}> From(object? value) {
                            return value switch
                            {
                            {{string.Join(", \n", Enumerable.Range(1, arity).Select(i => $"T{i} v{i} => new Any<{typeParams}>(v{i})"))}}, {{Environment.NewLine}}
                                _ => throw new InvalidCastException($"Cannot cast {value?.GetType().Name ?? "null"} to Any of ({{string.Join(", ", Enumerable
                                    .Range(1, arity)
                                    .Select(i => $$"""{typeof(T{{i}}).Name}"""))}})")
                            };
                        }
                        """);

        sb.AppendLine($$"""
                        /// <summary>
                        /// Creates a new Any from the given value.
                        /// </summary>
                        [MethodImpl(MethodImplOptions.AggressiveInlining)]
                        public static bool TryFrom(object? value, out Any<{{typeParams}}> result)
                        {
                            switch (value)
                            {
                                {{string.Join("\n", Enumerable.Range(1, arity).Select(i => $"""
                                     case T{i} v{i}:
                                       result = new Any<{typeParams}>(v{i});
                                       return true;
                                     
                                     """))}}
                                default:
                                    result = default;
                                    return false;
                                
                            }
                        }
                        """);
    }

    private static void EmitFromTnMethods(StringBuilder sb, int arity, string typeParams)
    {
        for (var i = 0; i < arity; i++)
        {
            sb.AppendLine($$"""
                            /// <summary>
                            /// Creates a new Any from the given value of type T{{i+1}}.
                            /// </summary>
                            [MethodImpl(MethodImplOptions.AggressiveInlining)]
                            public static Any<{{typeParams}}> FromT{{i+1}}(T{{i+1}}? value) {
                                return new Any<{{typeParams}}>(value);
                            }
                            """);
        }
    }
}