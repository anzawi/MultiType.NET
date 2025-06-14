using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(LTR), typeof(RTL), typeof(Auto))]
public partial struct DirectionMode
{
    public static DirectionMode From(string value) => value.ToLowerInvariant() switch
    {
        "ltr" => new LTR(),
        "rtl" => new RTL(),
        "auto" => new Auto(),
        _ => throw new ArgumentException("Invalid direction", nameof(value))
    };

    public bool IsLTR => this.Is<LTR>();
    public bool IsRTL => this.Is<RTL>();
    public bool IsAuto => this.Is<Auto>();

    public readonly struct LTR { }
    public readonly struct RTL { }
    public readonly struct Auto { }
}