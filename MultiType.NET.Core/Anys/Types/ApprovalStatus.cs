using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(Approved), typeof(Rejected), typeof(Pending), typeof(Canceled))]
public partial struct ApprovalStatus
{
    public static implicit operator ApprovalStatus(string value) =>
        value.ToLower() switch {
            "approved" => new Approved(),
            "rejected" => new Rejected(),
            "pending" => new Pending(),
            "canceled" => new Canceled(),
            _ => throw new ArgumentException("Invalid status")
        };

    public readonly struct Approved {}
    public readonly struct Rejected {}
    public readonly struct Pending {}
    public readonly struct Canceled {}
}