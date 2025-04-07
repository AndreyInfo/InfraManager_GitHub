using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class MassIncidentListFilter
    {
        public Guid[] GlobalIdentifiers { get; init; }
        public Guid? ReferenceID { get; init; }
        public ObjectClass? ObjectClass { get; init; }
    }
}
