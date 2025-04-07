using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests.RFCGantt;

public class RFCGanttDetails
{
    public ObjectClass ClassID { get; init; }
    public string FullName { get; init; }
    public Guid ID { get; init; }
    public Guid? ParentID { get; init; }
    public int Number { get; init; }
    public String Summary { get; init; }

    public String OwnerName { get; init; }
    public String CategoryName { get; init; }
    public String PriorityName { get; init; }
    public String PriorityColor { get; init; }
    //
    public String EntityStateName { get; init; }
    public String Description { get; init; }
    public String Target { get; init; }
    public String ServiceName { get; init; }
    public RFCUserDetails Initiator { get; init; }
    public RFCUserDetails Owner { get; init; }

    public Guid? QueueID { get; init; }
    public String QueueName { get; init; }

    public String UtcDateDetected { get; init; }
    public String UtcDatePromised { get; init; }
    public String UtcDateClosed { get; init; }
    public String UtcDateSolved { get; init; }
    public String UtcDateModified { get; init; }
    public String UtcDateStarted { get; init; }
}