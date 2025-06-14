using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(On), typeof(Off), typeof(Auto))]
public partial struct PowerState
{
    public static PowerState From(string value) => value.ToLowerInvariant() switch
    {
        "on" => new On(),
        "off" => new Off(),
        "auto" => new Auto(),
        _ => throw new ArgumentException("Invalid power state", nameof(value))
    };

    public bool IsOn => this.Is<On>();
    public bool IsOff => this.Is<Off>();
    public bool IsAuto => this.Is<Auto>();

    public readonly struct On { }
    public readonly struct Off { }
    public readonly struct Auto { }
}