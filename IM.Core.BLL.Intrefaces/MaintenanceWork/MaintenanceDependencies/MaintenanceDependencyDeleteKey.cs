using System;

namespace InfraManager.BLL.MaintenanceWork.MaintenanceDependencies;

public class MaintenanceDependencyDeleteKey
{
    public Guid MaintenanceID { get; init; }
    public Guid ObjectID { get; init; }
}
