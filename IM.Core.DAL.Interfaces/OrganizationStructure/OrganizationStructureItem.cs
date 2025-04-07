using System;

namespace InfraManager.DAL.OrganizationStructure;

public class OrganizationStructureItem
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public ObjectClass ClassID { get; init; }
}