using InfraManager.DAL.MaintenanceWork;
using System;


namespace InfraManager.BLL.MaintenanceWork.Maintenances;

public class MaintenanceDetails
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public Guid? MaintenanceFolderID { get; init; }
    public string MaintenanceFolderName { get; init; }
    public Guid WorkOrderTemplateID { get; init; }
    public MaintenanceState State { get; init; }
    public string StateName { get; init; }
    public MaintenanceMultiplicity Multiplicity { get; init; }
    public string MultiplicityName { get; init; }
    public byte[] RowVersion { get; init; }
    public string WorkOrderTemplateName { get; init; }
    public ObjectClass ClassID => ObjectClass.Maintenance;
}
