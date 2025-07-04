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

[JsonConverter(typeof(StatusTypeJsonConverter))]
public readonly partial struct StatusType : global::MultiType.NET.Core.IAny
{
    private readonly global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info> _inner;
    public StatusType(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info> value) => _inner = value;
    public static bool TryParse(string? input, IFormatProvider? _, out global::MultiType.NET.Core.Anys.Types.StatusType result)
    {
        var success = global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>.TryParse(input, _, out var inner);
        result = new MultiType.NET.Core.Anys.Types.StatusType(inner);
        return success;
    }

    public static implicit operator StatusType(MultiType.NET.Core.Anys.Types.StatusType.Success value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>.From(value));
    public static implicit operator StatusType(MultiType.NET.Core.Anys.Types.StatusType.Warning value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>.From(value));
    public static implicit operator StatusType(MultiType.NET.Core.Anys.Types.StatusType.Error value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>.From(value));
    public static implicit operator StatusType(MultiType.NET.Core.Anys.Types.StatusType.Info value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>.From(value));
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
    public Type[] AllowedTypes => global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>.AllowedTypes;
    /// <summary>
    /// The type of the value contained in this Any.
    /// </summary>
    public global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info> Inner => _inner;

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
    public static StatusType From(object? value) => new StatusType(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>.From(value));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFrom(object? value, out StatusType result)
    {
        var success = global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>.TryFrom(value, out var inner);
        result = new StatusType(inner);
        return success;
    }
}

public sealed class StatusTypeJsonConverter : JsonConverter<StatusType>
{
    private static readonly JsonConverter<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>> _innerConverter = (JsonConverter<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>>)(JsonSerializerOptions.Default.GetConverter(typeof(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>)) ?? throw new InvalidOperationException("Missing AnyJsonConverter."));
    public override StatusType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Use the configured converter (honors custom options)
        var inner = JsonSerializer.Deserialize<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StatusType.Success, global::MultiType.NET.Core.Anys.Types.StatusType.Warning, global::MultiType.NET.Core.Anys.Types.StatusType.Error, global::MultiType.NET.Core.Anys.Types.StatusType.Info>>(ref reader, options);
        return new StatusType(inner!);
    }

    public override void Write(Utf8JsonWriter writer, StatusType value, JsonSerializerOptions options)
    {
        // Avoid allocating unless needed
        JsonSerializer.Serialize(writer, value.Inner, options);
    }
}