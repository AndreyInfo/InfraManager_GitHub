using InfraManager.BLL.Asset;
using System;

namespace InfraManager.BLL.ServiceDesk.ServiceUnits;

public class ServiceUnitInsertDetails
{
    public string Name { get; init; }

    public Guid ResponsibleID { get; init; }

    public PerformerDetails[] Performers { get; set; }
}
