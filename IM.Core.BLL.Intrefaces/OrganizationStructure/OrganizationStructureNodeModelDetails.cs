using System;

namespace InfraManager.BLL.OrganizationStructure;

public class OrganizationStructureNodeModelDetails
{
    public string Name { get; init; }
    public string IconName { get; init; }
    public ObjectClass ClassID { get; init; }
    public Guid ObjectID { get; init; }
    public bool IsSelectFull { get; set; }
    public bool IsSelectPart { get; set; }
}
