using System;

namespace InfraManager.BLL.Location.Floors;

public class FloorData
{
    public string Name { get; init; }

    public string Note { get; init; }

    public int? BuildingID { get; init; }

    public int? ParentID { get; init; }

    public Guid? SubdivisionID { get; init; }

    public string ExternalID { get; init; }

    public byte[] RowVersion { get; init; }
}