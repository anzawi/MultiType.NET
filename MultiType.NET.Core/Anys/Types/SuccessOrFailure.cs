using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(Success), typeof(Failure))]
public readonly partial struct SuccessOrFailure
{
    public static SuccessOrFailure From(bool value) =>
        value ? new Success() : new Failure();

    public static SuccessOrFailure From(Success value) => value;
    public static SuccessOrFailure From(Failure value) => value;

    public static SuccessOrFailure From(string value)
    {
        if (value.Equals("success", StringComparison.OrdinalIgnoreCase))
            return new Success();
        if (value.Equals("fail", StringComparison.OrdinalIgnoreCase)
            || value.Equals("failure", StringComparison.OrdinalIgnoreCase))
            return new Failure();

        throw new ArgumentException("Invalid value", nameof(value));
    }

    public bool IsSuccess => this.Is<Success>();
    public bool IsFail => this.Is<Failure>();

    public static implicit operator SuccessOrFailure(bool value) => From(value);

    public readonly struct Success
    {
    }

    public readonly struct Failure
    {
    }
    
}