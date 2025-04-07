using System;

namespace InfraManager.DAL.WorkFlow.Events;

public class EntityEventListItem : BaseEventItem
{
    public string EntityFullName { get; init; }
    
    public string TargetStateID { get; init; }
    
    public Guid EntityID { get; init; }
    
    public ObjectClass EntityClassID { get; init; }
    
    public byte[] Argument { get; init; }
    
    public Guid? WorkflowSchemeID { get; init; }
    
    public string WorkflowSchemeFullName { get; init; }
}