using System;

namespace InfraManager.BLL.MaintenanceWork.MaintenanceDependencies;
public class MaintenanceDependencyData
{
    public Guid MaintenanceID { get; init; }
    public Guid ObjectID { get; init; }
    public string ObjectName { get; init; }
    public string ObjectLocation { get; init; }
    public ObjectClass ObjectClassID { get; init; }
    public string Note { get; init; }
    public byte Type { get; init; }
    public bool Locked { get; init; }
    public string ClassName { get; init; }
}
