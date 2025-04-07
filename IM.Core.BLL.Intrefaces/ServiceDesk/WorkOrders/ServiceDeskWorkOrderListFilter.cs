using System;
using System.Collections.Generic;

namespace InfraManager.BLL.ServiceDesk.WorkOrders;

public class ServiceDeskWorkOrderListFilter
{
    public Guid? InitiatorID { get; init; }
    public Guid? TypeID { get; init; }
    public int? Number { get; init; }
    public IReadOnlyList<Guid> Ids { get; init; }
    public long ReferenceID { get; init; }
    public IEnumerable<long> ReferenceIDs { get; init; }
    public bool? ShouldSearchFinished { get; init; }
    public Guid? ReferencedObjectID { get; init; }
    public ObjectClass? ReferencedObjectClassID { get; init; }
    public bool? ShouldSearchAccomplished { get; init; }
    public Guid? ExecutorID { get; init; }
    public DateTime? UtcDateModified { get; init; }
}