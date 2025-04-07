using System;
using InfraManager.BLL.Asset;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure.Groups;

public class GroupData
{
    public string Name { get; init; }
    public string Used { get; init; } = "";
    public string Note { get; init; }
    public GroupTypeData Type { get; init; }
    public Guid ResponsibleUserID { get; init; }

    public Guid[] PerformersID { get; init; }
}
