using System;

namespace InfraManager.BLL.Messages;

public class MessageDetails
{
    public Guid IMObjID { get; init; }
    public DateTime UtcDateRegistered { get; init; }
    public string EntityStateID { get; init; }
    public string TargetEntityStateID { get; init; }
    public Guid? WorkflowSchemeID { get; init; }
    public string WorkflowSchemeVersion { get; init; }
    public string WorkflowSchemeIdentifier { get; init; }
    public string EntityStateName { get; init; }
    public DateTime? UtcDateClosed { get; init; }
    public byte Type { get; init; }
    public string TypeName { get; init; }
    public int Count { get; init; }
    public byte SeverityType { get; init; }
    public string SeverityName { get; init; }
    public byte[] RowVersion { get; init; }
}
