using System;

namespace InfraManager.DAL.ServiceCatalogue.SLA;

public class SLAConcludedWithItem
{
    public ObjectClass ObjectClass { get; init; }
    public string ConcludedWith { get; init; }
    public Guid ObjectID { get; init; }
}