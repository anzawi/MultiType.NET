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

[JsonConverter(typeof(WorkflowStatusJsonConverter))]
public readonly partial struct WorkflowStatus : global::MultiType.NET.Core.IAny
{
    private readonly global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived> _inner;
    public WorkflowStatus(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived> value) => _inner = value;
    public static bool TryParse(string? input, IFormatProvider? _, out global::MultiType.NET.Core.Anys.Types.WorkflowStatus result)
    {
        var success = global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>.TryParse(input, _, out var inner);
        result = new MultiType.NET.Core.Anys.Types.WorkflowStatus(inner);
        return success;
    }

    public static implicit operator WorkflowStatus(MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>.From(value));
    public static implicit operator WorkflowStatus(MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>.From(value));
    public static implicit operator WorkflowStatus(MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>.From(value));
    public static implicit operator WorkflowStatus(MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>.From(value));
    public static implicit operator WorkflowStatus(MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived value) => new(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>.From(value));
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
    public Type[] AllowedTypes => global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>.AllowedTypes;
    /// <summary>
    /// The type of the value contained in this Any.
    /// </summary>
    public global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived> Inner => _inner;

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
    public static WorkflowStatus From(object? value) => new WorkflowStatus(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>.From(value));
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryFrom(object? value, out WorkflowStatus result)
    {
        var success = global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>.TryFrom(value, out var inner);
        result = new WorkflowStatus(inner);
        return success;
    }
}

public sealed class WorkflowStatusJsonConverter : JsonConverter<WorkflowStatus>
{
    private static readonly JsonConverter<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>> _innerConverter = (JsonConverter<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>>)(JsonSerializerOptions.Default.GetConverter(typeof(global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>)) ?? throw new InvalidOperationException("Missing AnyJsonConverter."));
    public override WorkflowStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Use the configured converter (honors custom options)
        var inner = JsonSerializer.Deserialize<global::MultiType.NET.Core.Anys.Generated.Any<global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Draft, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Submitted, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Approved, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Rejected, global::MultiType.NET.Core.Anys.Types.WorkflowStatus.Archived>>(ref reader, options);
        return new WorkflowStatus(inner!);
    }

    public override void Write(Utf8JsonWriter writer, WorkflowStatus value, JsonSerializerOptions options)
    {
        // Avoid allocating unless needed
        JsonSerializer.Serialize(writer, value.Inner, options);
    }
}