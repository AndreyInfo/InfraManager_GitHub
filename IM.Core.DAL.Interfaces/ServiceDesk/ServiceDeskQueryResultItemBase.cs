using System;

namespace InfraManager.DAL.ServiceDesk;

public class ServiceDeskQueryResultItemBase
{
    public Guid ID { get; init; }
    public Guid TypeID { get; init; }
    public int Number { get; init; }
    public Guid? WorkflowSchemeID { get; init; }
    public string WorkflowSchemeIdentifier { get; init; }
    public string WorkflowSchemeVersion { get; init; }
    public string EntityStateID { get; init; }
    public string EntityStateName { get; init; }
}