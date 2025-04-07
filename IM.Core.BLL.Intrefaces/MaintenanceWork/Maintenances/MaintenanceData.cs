using InfraManager.DAL.MaintenanceWork;
using System;

namespace InfraManager.BLL.MaintenanceWork.Maintenances;

public class MaintenanceData
{
    public string Name { get; init; }

    public string Note { get; init; }

    public Guid? MaintenanceFolderID { get; init; }

    public MaintenanceState State { get; init; }

    public MaintenanceMultiplicity Multiplicity { get; init; }

    public byte[] RowVersion { get; init; }

    public Guid WorkOrderTemplateID { get; init; }
}
