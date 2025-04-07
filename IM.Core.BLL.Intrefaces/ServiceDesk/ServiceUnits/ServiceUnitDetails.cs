using InfraManager.BLL.Asset;
using System;

namespace InfraManager.BLL.ServiceDesk.ServiceUnits;

public class ServiceUnitDetails
{
    public Guid ID { get; init; }

    public string Name { get; init; }

    public Guid ResponsibleID { get; init; }

    public byte[] RowVersion { get; init; }

    public string ResponsibleName { get; init; }

    public PerformerDetails[] Performers { get; set; }
}
