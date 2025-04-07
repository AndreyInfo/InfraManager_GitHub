using System;

namespace InfraManager.BLL.ServiceDesk;

public class ServiceDeskListItemBase
{
    public Guid ID { get; init; }
    public ObjectClass ClassID { get; init; }
    public Guid? WorkflowSchemeID { get; init; }
    public string WorkflowSchemeIdentifier { get; init; }
    public string WorkflowSchemeVersion { get; init; }
    public string EntityStateID { get; init; }
}