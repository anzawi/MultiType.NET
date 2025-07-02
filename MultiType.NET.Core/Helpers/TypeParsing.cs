using System.Globalization;

namespace MultiType.NET.Core.Helpers;

public static class TypeParsing
{
    public static bool IsPrimitiveType(this Type targetType, string? input, out object? value,
        IFormatProvider? formatProvider = null)
    {
        value = null;

        if (string.IsNullOrWhiteSpace(input))
            return false;

        try
        {
            var nullableUnderlying = Nullable.GetUnderlyingType(targetType);
            var actualType = nullableUnderlying ?? targetType;
            formatProvider ??= CultureInfo.InvariantCulture;

            if (actualType == typeof(string))
            {
                value = input;
                return true;
            }

            if (actualType == typeof(int) && int.TryParse(input, NumberStyles.Any, formatProvider, out var i))
            {
                value = i;
                return true;
            }

            if (actualType == typeof(long) && long.TryParse(input, NumberStyles.Any, formatProvider, out var l))
            {
                value = l;
                return true;
            }

            if (actualType == typeof(bool) && bool.TryParse(input, out var b))
            {
                value = b;
                return true;
            }

            if (actualType == typeof(double) && double.TryParse(input, NumberStyles.Any, formatProvider, out var d))
            {
                value = d;
                return true;
            }

            if (actualType == typeof(float) && float.TryParse(input, NumberStyles.Any, formatProvider, out var f))
            {
                value = f;
                return true;
            }

            if (actualType == typeof(decimal) && decimal.TryParse(input, NumberStyles.Any, formatProvider, out var dec))
            {
                value = dec;
                return true;
            }

            if (actualType == typeof(DateTime) &&
                DateTime.TryParse(input, formatProvider, DateTimeStyles.None, out var dt))
            {
                value = dt;
                return true;
            }

            if (actualType == typeof(Guid) && Guid.TryParse(input, out var guid))
            {
                value = guid;
                return true;
            }

            if (actualType == typeof(TimeSpan) && TimeSpan.TryParse(input, formatProvider, out var ts))
            {
                value = ts;
                return true;
            }

            if (actualType == typeof(Version))
            {
                try
                {
                    value = new Version(input);
                    return true;
                }
                catch
                {
                    // invalid version format
                }
            }

            if (actualType == typeof(Uri))
            {
                if (Uri.TryCreate(input, UriKind.RelativeOrAbsolute, out var uri))
                {
                    value = uri;
                    return true;
                }
            }

            if (actualType.IsEnum && Enum.TryParse(actualType, input, ignoreCase: true, out var enumVal))
            {
                value = enumVal;
                return true;
            }
        }
        catch
        {
            // Ignore parse errors
        }

        return false;
    }
}