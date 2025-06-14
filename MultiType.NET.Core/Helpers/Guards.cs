namespace MultiType.NET.Core.Helpers;

using System.Linq.Expressions;
using Exceptions;

internal static class Guards
{
    internal static void ThrowIfNotInitialized(byte typeIndex, Type valueType)
    {
        if (typeIndex == 0)
            throw new InvalidAnyStateException(
                $"""
                 Any is in an uninitialized state
                 Expected: Value of type {valueType.Name}
                 Actual: Uninitialized
                 """);
    }

    internal static void ThrowIfNotOutOfRange(byte typeIndex, int maximum)
    {
        if (typeIndex > maximum)
            throw new InvalidAnyStateException(
                $"""
                 Invalid type index
                 Maximum allowed: 3
                 Actual: {typeIndex}
                 """);
    }

    internal static void ThrowIfNull(object? value, Type type, byte typeIndex)
    {
        if (value is null)
            throw new InvalidAnyStateException(
                $"""
                 Any value is null
                 Expected type: {type.Name}
                 TypeIndex: {typeIndex}
                 """);
    }
    
    internal static void ThrowIfTypeMismatch<TExpected>(byte typeIndex, byte expectedTypeIndex, object? value, out TExpected result)
    {
        if (typeIndex != expectedTypeIndex || value is not TExpected typedValue)
        {
            result = default!;
            throw new InvalidAnyStateException(
                $"""
                 Type mismatch in Any
                 Expected: {typeof(TExpected).Name}
                 Actual: {value?.GetType().Name}
                 TypeIndex: {typeIndex}
                 """);
        }

        result = typedValue;
    }

}