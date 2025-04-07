using System;

namespace InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;

public class ProblemDependencyListItem
{
    public long ID { get; init; }
    public Guid EntityID { get; init; }
    public Guid ObjectID { get; init; }
    public string Name { get; init; }
    public string Location { get; init; }
    public string ClassName { get; init; }
    public string Note { get; init; }
    public bool Locked { get; init; }
    public ObjectClass ClassID { get; init; }
}