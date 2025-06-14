using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(Male), typeof(Female))]
public partial struct MaleFemale
{
    public static MaleFemale From(Male value) => value;
    public static MaleFemale From(Female value) => value;

    public bool IsMale => this.Is<Male>();
    public bool IsFemale => this.Is<Female>();
    
    public readonly struct Male
    {
    }

    public readonly struct Female
    {
    }
}