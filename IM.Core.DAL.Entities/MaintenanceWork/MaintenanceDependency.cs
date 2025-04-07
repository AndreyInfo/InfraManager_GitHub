using Inframanager;
using InfraManager.DAL.Settings;
using System;

namespace InfraManager.DAL.MaintenanceWork;

[OperationIdMapping(ObjectAction.Insert, OperationID.Maintenance_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Maintenance_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Maintenance_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Maintenance_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Maintenance_Properties)]
public class MaintenanceDependency
{
    public MaintenanceDependency(Guid maintenanceID, Guid objectID)
    {
        ObjectID = objectID;
        MaintenanceID = maintenanceID;
        Maintenance = null;
        ObjectClass = null;
    }

    public Guid MaintenanceID { get; init; }
    public Guid ObjectID { get; init; }
    public string ObjectName { get; init; }
    public string ObjectLocation { get; init; }
    public ObjectClass ObjectClassID { get; init; }
    public string Note { get; init; }
    public byte Type { get; init; }
    public bool Locked { get; init; }

    public virtual Maintenance Maintenance { get; init; }
    public virtual InframanagerObjectClass ObjectClass { get; init; }
}
