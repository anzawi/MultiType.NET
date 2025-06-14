using MultiType.NET.Core.Attributes;

namespace MultiType.NET.Core.Anys.Types;

[GenerateAny(typeof(Draft), typeof(Submitted), typeof(Approved), typeof(Rejected), typeof(Archived))]
public partial struct WorkflowStatus
{
    public static WorkflowStatus From(string status) => status.ToLowerInvariant() switch
    {
        "draft" => new Draft(),
        "submitted" => new Submitted(),
        "approved" => new Approved(),
        "rejected" => new Rejected(),
        "archived" => new Archived(),
        _ => throw new ArgumentException("Invalid workflow status", nameof(status))
    };

    public bool IsDraft => this.Is<Draft>();
    public bool IsSubmitted => this.Is<Submitted>();
    public bool IsApproved => this.Is<Approved>();
    public bool IsRejected => this.Is<Rejected>();
    public bool IsArchived => this.Is<Archived>();

    public readonly struct Draft { }
    public readonly struct Submitted { }
    public readonly struct Approved { }
    public readonly struct Rejected { }
    public readonly struct Archived { }
}
