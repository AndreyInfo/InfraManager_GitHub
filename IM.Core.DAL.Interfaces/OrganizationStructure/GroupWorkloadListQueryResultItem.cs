using System;

namespace InfraManager.DAL.OrganizationStructure;

public class GroupWorkloadListQueryResultItem
{
    public Guid ID { get; init; }
    public string FullName { get; init; }
    public int Size { get; init; }
}