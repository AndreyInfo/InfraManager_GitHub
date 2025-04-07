using System;

namespace InfraManager.BLL.ServiceDesk.HandlingTechnicalFailures;

[Obsolete]
public class HandlingTechnicalFailureFilter
{
    public Guid? ServiceID { get; init; }
    public int? CategoryID { get; init; }
}
