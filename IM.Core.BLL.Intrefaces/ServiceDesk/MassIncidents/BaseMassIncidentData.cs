using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class BaseMassIncidentData
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public Guid? PriorityID { get; init; }
        public Guid? CriticalityID { get; init; }        
        public string Solution { get; init; }
        public string Cause { get; init; }
        public Guid? SlaID { get; init; }
        public int? TechnicalFailureCategoryID { get; init; }
    }
}
