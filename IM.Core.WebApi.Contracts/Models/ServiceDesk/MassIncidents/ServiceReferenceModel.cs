using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents
{
    public class ServiceReferenceModel
    {
        public long ID { get; init; }
        public Guid MassIncidentID { get; init; }
        public Guid ReferenceID { get; init; }
        public string ServiceUri { get; init; }
    }
}
