using System;

namespace InfraManager.BLL.Location.Workplaces;

public class WorkplaceDetails : WorkplaceData
{
    public int ID { get; init; }

    public Guid IMObjID { get; init; }
}