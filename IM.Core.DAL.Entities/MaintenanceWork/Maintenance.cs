using Inframanager;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.MaintenanceWork;

[ObjectClassMapping(ObjectClass.Maintenance)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Maintenance_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.Maintenance_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Maintenance_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Maintenance_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Maintenance_Properties)]
public class Maintenance
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public string Note { get; init; }
    public Guid? FolderID { get; init; }
    public Guid WorkOrderTemplateID { get; init; }
    public MaintenanceState State { get; init; }
    public MaintenanceMultiplicity Multiplicity { get; init; }
    public byte[] RowVersion { get; init; }

    public virtual WorkOrderTemplate WorkOrderTemplate { get; init; }
    public virtual MaintenanceFolder Folder { get; init; }
    public virtual ICollection<MaintenanceDependency> MaintenanceDependencies { get; init; }
}

