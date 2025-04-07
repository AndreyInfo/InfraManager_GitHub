using System;
using InfraManager.DAL.Events;

namespace InfraManager.DAL.WorkFlow.Events;

public class BaseEventItem
{
    public Guid ID { get; init; }

    public DateTime UtcRegisteredAt { get; init; }

    public long Order { get; init; }

    public EventSource Source { get; init; }

    public EventType Type { get; init; }

    public Guid? OwnerID { get; init; }

    public DateTime? UtcOwnedUntil { get; init; }

    public Guid CauserID { get; init; }

    public bool IsProcessed { get; init; }
    
    public string CauserFullName { get; init; }
}