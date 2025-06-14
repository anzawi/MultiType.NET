using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(Ascending), typeof(Descending), typeof(None))]
public partial struct SortOrder
{
    public static SortOrder From(string value) => value.ToLowerInvariant() switch
    {
        "asc" or "ascending" => new Ascending(),
        "desc" or "descending" => new Descending(),
        "none" => new None(),
        _ => throw new ArgumentException("Invalid sort order", nameof(value))
    };

    public bool IsAscending => this.Is<Ascending>();
    public bool IsDescending => this.Is<Descending>();
    public bool IsNone => this.Is<None>();

    public readonly struct Ascending { }
    public readonly struct Descending { }
    public readonly struct None { }
}