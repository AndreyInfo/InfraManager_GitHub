using Inframanager;
using System;
using System.Collections.Generic;

namespace InfraManager.DAL.MaintenanceWork;

/// <summary>
/// Папка для регламентный работ
/// </summary>
[ObjectClassMapping(ObjectClass.MaintenanceFolder)]
[OperationIdMapping(ObjectAction.Insert, OperationID.MaintenanceFolder_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.MaintenanceFolder_Update)]
[OperationIdMapping(ObjectAction.Delete, OperationID.MaintenanceFolder_Delete)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.MaintenanceFolder_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.MaintenanceFolder_Properties)]
public class MaintenanceFolder
{
    public Guid ID { get; init; }
    public string Name { get; init; }
    public Guid? ParentID { get; init; }
    public byte[] RowVersion { get; init; }

    public virtual MaintenanceFolder Parent { get; init; }
    public virtual ICollection<MaintenanceFolder> SubFolders { get; init; }
    public virtual ICollection<Maintenance> Maintenances { get; init; }
}
