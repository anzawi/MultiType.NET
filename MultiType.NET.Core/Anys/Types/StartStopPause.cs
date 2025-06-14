using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(Start), typeof(Stop), typeof(Pause))]
public partial struct StartStopPause
{
    public static StartStopPause From(bool value) =>
        value ? new Start() : new Stop();

    public static StartStopPause From(Start value) => value;
    public static StartStopPause From(Stop value) => value;
    public static StartStopPause From(Pause value) => value;

    public bool IsStarted => this.Is<Start>();
    public bool IsStopped => this.Is<Stop>();
    public bool IsPaused => this.Is<Pause>();

    public static StartStopPause ToStart() => From(new Start());
    public static StartStopPause ToStop() => From(new Stop());
    public static StartStopPause ToPause() => From(new Pause());

    public static implicit operator StartStopPause(bool value) => From(value);
    
    public readonly struct Start
    {
    }

    public readonly struct Stop
    {
    }

    public readonly struct Pause
    {
    }
}