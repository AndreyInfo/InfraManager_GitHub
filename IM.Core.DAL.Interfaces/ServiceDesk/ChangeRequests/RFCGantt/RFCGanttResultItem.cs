using System;
using InfraManager.DAL.ServiceDesk.ChangeRequests.RFCGantt;

namespace InfraManager.DAL.Interface.ServiceDesk.ChangeRequests.RFCGantt;

public sealed class RFCGanttResultItem
{
    public Guid ID { get; init; }
    public int Number { get; init; }
    public ObjectClass TypeClass { get; init; }
    public String Summary { get; init; }
    public String CategoryName { get; init; }
    public String OwnerName { get; init; }
    public String PriorityName { get; init; }
    public int PriorityColor { get; init; }
    public Guid? ParentID { get; init; }
    public Guid? InitiatorID { get; init; }
    public RFCGanttUserResultItem Initiator { get; init; }
    public Guid? OwnerID { get; init; } // lля WO подставляется исполнитель
    public String OwnerFullName { get; init; }
    public RFCGanttUserResultItem Owner { get; init; }
    public Guid? QueueID { get; init; }
    public String QueueName { get; init; }

    public String EntityStateName { get; init; }
    public String Description { get; init; }
    public String Target { get; init; }
    public String ServiceName { get; init; }
    //
    public DateTime UtcDateDetected { get; init; }
    public DateTime? UtcDatePromised { get; init; }
    public DateTime? UtcDateClosed { get; init; }
    public DateTime? UtcDateSolved { get; init; }
    public DateTime UtcDateModified { get; init; }
    public DateTime? UtcDateStarted { get; init; }
}