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

[JsonConverter(typeof(StartStopPauseJsonConverter))]
public readonly partial struct StartStopPause : global::MultiType.NET.Core.IAny
{
    private readonly global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause> _inner;
    public StartStopPause(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause> value) => _inner = value;
    public static bool TryParse(string? input, IFormatProvider? _, out global::MultiType.NET.Core.Anys.Types.StartStopPause result)
    {
        var success = global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>.TryParse(input, _, out var inner);
        result = new MultiType.NET.Core.Anys.Types.StartStopPause(inner);
        return success;
    }

    public static implicit operator StartStopPause(MultiType.NET.Core.Anys.Types.StartStopPause.Start value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>.From(value));
    public static implicit operator StartStopPause(MultiType.NET.Core.Anys.Types.StartStopPause.Stop value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>.From(value));
    public static implicit operator StartStopPause(MultiType.NET.Core.Anys.Types.StartStopPause.Pause value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>.From(value));
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
    public Type[] AllowedTypes => global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>.AllowedTypes;
    /// <summary>
    /// The type of the value contained in this Any.
    /// </summary>
    public global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause> Inner => _inner;

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
    public static StartStopPause From(object? value) => new StartStopPause(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>.From(value));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFrom(object? value, out StartStopPause result)
    {
        var success = global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>.TryFrom(value, out var inner);
        result = new StartStopPause(inner);
        return success;
    }
}

public sealed class StartStopPauseJsonConverter : JsonConverter<StartStopPause>
{
    private static readonly JsonConverter<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>> _innerConverter = (JsonConverter<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>>)(JsonSerializerOptions.Default.GetConverter(typeof(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>)) ?? throw new InvalidOperationException("Missing AnyJsonConverter."));
    public override StartStopPause Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Use the configured converter (honors custom options)
        var inner = JsonSerializer.Deserialize<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.StartStopPause.Start, global::MultiType.NET.Core.Anys.Types.StartStopPause.Stop, global::MultiType.NET.Core.Anys.Types.StartStopPause.Pause>>(ref reader, options);
        return new StartStopPause(inner!);
    }

    public override void Write(Utf8JsonWriter writer, StartStopPause value, JsonSerializerOptions options)
    {
        // Avoid allocating unless needed
        JsonSerializer.Serialize(writer, value.Inner, options);
    }
}