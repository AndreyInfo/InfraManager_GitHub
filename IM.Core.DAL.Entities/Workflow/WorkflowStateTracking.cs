using System;

namespace InfraManager.DAL.WF;

public class WorkflowStateTracking
{
    public Guid WorkflowTrackingId { get; init; }

    public string StateId { get; set; }

    public string StateName { get; set; }

    public Guid? ExecutorId { get; set; }

    public DateTime? UtcLeavedAt { get; set; }

    public DateTime UtcEnteredAt { get; set; }

    public int? TimeSpanInWorkMinutes { get; set; }
}