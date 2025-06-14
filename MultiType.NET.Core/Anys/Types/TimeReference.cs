using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(Past), typeof(Present), typeof(Future))]
public partial struct TimeReference
{
    public static TimeReference From(DateTime dateTime)
    {
        var now = DateTime.UtcNow;
        if (dateTime < now) return new Past();
        if (dateTime > now) return new Future();
        return new Present();
    }

    public bool IsPast => this.Is<Past>();
    public bool IsPresent => this.Is<Present>();
    public bool IsFuture => this.Is<Future>();

    public readonly struct Past { }
    public readonly struct Present { }
    public readonly struct Future { }
}