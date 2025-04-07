using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents;

public class MassIncidentsReportItemModel
{
    public int ID { get; init; }
    public Guid IMObjID { get; init; }
    public int Number { get; init; }
    public string Type { get; init; }
    public string Summary { get; init; }
    public string ServiceName { get; init; }
    public string Cause { get; init; }
    public string Owner { get; init; }
    public string Priority { get; init; }
    public string EntityStateName { get; init; }
    public string Initiator { get; init; }
    public string UtcCreatedAt { get; init; }
    public string UtcLastModifiedAt { get; init; }
}