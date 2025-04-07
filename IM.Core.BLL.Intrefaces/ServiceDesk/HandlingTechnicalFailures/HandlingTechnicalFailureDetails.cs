using System;

namespace InfraManager.BLL.ServiceDesk.HandlingTechnicalFailures;

[Obsolete]
public class HandlingTechnicalFailureDetails : HandlingTechnicalFailureData
{
    public Guid ID { get; init; }

    public string CategoryName { get; init; }
}