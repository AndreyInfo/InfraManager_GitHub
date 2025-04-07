using System;

namespace InfraManager.BLL.Asset;

public class PerformerDetails
{
    public ObjectClass ClassID { get; init; }

    public int? ID { get; init; }

    public Guid UID { get; init; }

    public Guid ServiceUnitID { get; set; }

    public string Name { get; init; }

    public string ResponsibleName { get; init; }

    public string Email { get; init; }

    public string Phone { get; init; }

    public string DepartamentName { get; init; }

    public string PositionName { get; init; }

    public byte[] Photo { get; init; }
}
