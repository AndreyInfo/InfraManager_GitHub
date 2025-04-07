using System;

namespace InfraManager.BLL.Location.Buildings;

public class BuildingListFilter
{
    public string Name { get; init; }

    public Guid? IMObjID { get; init; }

    public Guid? OrganizationID { get; init; }
}