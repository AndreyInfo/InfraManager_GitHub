using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class MassIncidentReferenceDetails
    {
        public long ID { get; set; }
        public Guid MassIncidentID { get; init; }
        public Guid ReferenceID { get; set; }
    }
}
