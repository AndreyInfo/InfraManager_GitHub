using InfraManager.DAL.AccessManagement;
using System;

namespace InfraManager.BLL.OrganizationStructure;

public class OrganizationStructureNodeRequestModelDetails
{
    public bool IncludeUsers { get; init; }

    public ObjectClass ClassID { get; init; }

    public Guid? ObjectID { get; init; }

    public Guid? OwnerID { get; init; }
}
