using System;

namespace InfraManager.DAL.ServiceDesk.MassIncidents;

public class MassIncidentsReportQueryResultItem
{
    public int ID { get; init; }
    public Guid IMObjID { get; init; }
    public string Type { get; init; }
    public string Summary { get; init; }
    public string ServiceName { get; init; }
    public string Cause { get; init; }
    public string Owner { get; init; }
    public string Initiator { get; init; }
    public string Priority { get; init; }
    public string EntityStateName { get; init; }
    public DateTime UtcCreatedAt { get; init; }
    public DateTime UtcLastModifiedAt { get; init; }
}