using System;

namespace InfraManager.BLL.ServiceDesk.ServiceUnits;

public class PerformerServiceUnitDetails
{
    public Guid PerformerID { get; init; }
    public Guid ServiceUnitID { get; init; }
    
    public int ClassID { get; init; }
}
