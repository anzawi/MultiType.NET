using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(True), typeof(False))]
public partial struct YesOrNo
{
    public static YesOrNo From(bool value) =>
        value ? new True() : new False();

    public static YesOrNo From(True value) => value;
    public static YesOrNo From(False value) => value;

    public static YesOrNo From(string value)
    {
        if (value.Equals("Yes", StringComparison.OrdinalIgnoreCase))
            return new True();
        if (value.Equals("no", StringComparison.OrdinalIgnoreCase))
            return new False();

        throw new ArgumentException("Invalid value", nameof(value));
    }

    public bool Yes => this.Is<True>();
    public bool No => this.Is<False>();

    public static implicit operator YesOrNo(bool value) => From(value);

    public readonly struct True
    {
    }

    public readonly struct False
    {
    }
}