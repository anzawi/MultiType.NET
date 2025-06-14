using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(Enabled), typeof(Disabled))]
public partial struct EnabledDisabled
{
    public static EnabledDisabled From(bool value) => value ? new Enabled() : new Disabled();

    public bool IsEnabled => this.Is<Enabled>();
    public bool IsDisabled => this.Is<Disabled>();

    public static implicit operator EnabledDisabled(bool value) => From(value);

    public readonly struct Enabled { }
    public readonly struct Disabled { }
}