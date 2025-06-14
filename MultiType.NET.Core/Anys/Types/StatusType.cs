using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(Success), typeof(Warning), typeof(Error), typeof(Info))]
public partial struct StatusType
{
    public static StatusType From(string value) => value.ToLowerInvariant() switch
    {
        "success" => new Success(),
        "warning" => new Warning(),
        "error" => new Error(),
        "info" => new Info(),
        _ => throw new ArgumentException("Invalid status", nameof(value))
    };

    public bool IsSuccess => this.Is<Success>();
    public bool IsWarning => this.Is<Warning>();
    public bool IsError => this.Is<Error>();
    public bool IsInfo => this.Is<Info>();

    public readonly struct Success { }
    public readonly struct Warning { }
    public readonly struct Error { }
    public readonly struct Info { }
}