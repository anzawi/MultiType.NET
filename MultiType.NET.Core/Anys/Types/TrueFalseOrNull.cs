using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(True), typeof(False), typeof(Null))]
public partial struct TrueFalseOrNull
{
    public static TrueFalseOrNull From(bool? value) =>
        value is null ? new Null() :
        value.Value ? new True() : new False();

    public static TrueFalseOrNull From(True value) =>
        new True();

    public static TrueFalseOrNull From(False value) =>
        new False();

    public static TrueFalseOrNull From(Null value) =>
        new Null();

    public bool IsTrue => this.Is<True>();
    public bool IsFalse => this.Is<False>();
    public bool IsNoValue => this.Is<Null>();

    public static implicit operator TrueFalseOrNull(bool? value) => From(value);

    public readonly struct True
    {
    }

    public readonly struct False
    {
    }

    public readonly struct Null
    {
    }
}