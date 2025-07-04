﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by MultiType.NET.SourceGenerator
//     Library Version: {LibraryVersion}
//     Runtime Version: {RuntimeVersion}
//     Generated: {GenerationTime}
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
#pragma warning disable 1591
namespace MultiType.NET.Core.Anys.Types;
using global::System.Runtime.CompilerServices;
using global::System.Text.Json.Serialization;
using global::MultiType.NET.Core.Serialization.Generated;
using global::System.Text.Json;

[JsonConverter(typeof(TrueFalseOrNullJsonConverter))]
public readonly partial struct TrueFalseOrNull : global::MultiType.NET.Core.IAny
{
    private readonly global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null> _inner;
    public TrueFalseOrNull(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null> value) => _inner = value;
    public static bool TryParse(string? input, IFormatProvider? _, out global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull result)
    {
        var success = global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>.TryParse(input, _, out var inner);
        result = new MultiType.NET.Core.Anys.Types.TrueFalseOrNull(inner);
        return success;
    }

    public static implicit operator TrueFalseOrNull(MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>.From(value));
    public static implicit operator TrueFalseOrNull(MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>.From(value));
    public static implicit operator TrueFalseOrNull(MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>.From(value));
    public byte TypeIndex => _inner.TypeIndex;
    /// <inheritdoc/>
    public object? Value => _inner.Value;
    /// <inheritdoc/>
    public Type Type => _inner.Type;
    /// <inheritdoc/>
    public bool HasValue => _inner.HasValue;
    /// <inheritdoc/>
    public bool IsNull => _inner.IsNull;
    /// <summary>
    /// The set of allowed types for this Any.
    /// </summary>
    public Type[] AllowedTypes => global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>.AllowedTypes;
    /// <summary>
    /// The type of the value contained in this Any.
    /// </summary>
    public global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null> Inner => _inner;

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Is<T>() => _inner.Is<T>();
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T As<T>() => _inner.As<T>();
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? AsNullable<T>()
        where T : class => _inner.AsNullable<T>();
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T? AsNullableStruct<T>()
        where T : struct => _inner.AsNullableStruct<T>();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TrueFalseOrNull From(object? value) => new TrueFalseOrNull(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>.From(value));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFrom(object? value, out TrueFalseOrNull result)
    {
        var success = global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>.TryFrom(value, out var inner);
        result = new TrueFalseOrNull(inner);
        return success;
    }
}

public sealed class TrueFalseOrNullJsonConverter : JsonConverter<TrueFalseOrNull>
{
    private static readonly JsonConverter<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>> _innerConverter = (JsonConverter<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>>)(JsonSerializerOptions.Default.GetConverter(typeof(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>)) ?? throw new InvalidOperationException("Missing AnyJsonConverter."));
    public override TrueFalseOrNull Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Use the configured converter (honors custom options)
        var inner = JsonSerializer.Deserialize<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.True, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.False, global::MultiType.NET.Core.Anys.Types.TrueFalseOrNull.Null>>(ref reader, options);
        return new TrueFalseOrNull(inner!);
    }

    public override void Write(Utf8JsonWriter writer, TrueFalseOrNull value, JsonSerializerOptions options)
    {
        // Avoid allocating unless needed
        JsonSerializer.Serialize(writer, value.Inner, options);
    }
}