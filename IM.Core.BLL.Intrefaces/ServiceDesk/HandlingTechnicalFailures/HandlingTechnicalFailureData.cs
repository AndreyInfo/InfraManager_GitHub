using System;

namespace InfraManager.BLL.ServiceDesk.HandlingTechnicalFailures;

[Obsolete]
public class HandlingTechnicalFailureData
{
    public Guid ServiceID { get; init; }
    public int CategoryID { get; init; }
    public Guid GroupID { get; init; }
}