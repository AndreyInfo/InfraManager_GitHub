using System;
using Inframanager;

namespace InfraManager.DAL.Dashboards;

[ObjectClassMapping(ObjectClass.DashboardFolder)]
[OperationIdMapping(ObjectAction.Delete, OperationID.DashboardFolder_Delete)]
[OperationIdMapping(ObjectAction.Insert, OperationID.DashboardFolder_Add)]
[OperationIdMapping(ObjectAction.Update, OperationID.DashboardFolder_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.DashboardFolder_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.DashboardFolder_Properties)]
public class DashboardFolder
{
    public Guid ID { get; init; }
    
    public string Name { get; init; }
    
    public Guid? ParentDashboardFolderID { get; init; }

    public byte[] RowVersion { get; init; }

    public virtual DashboardFolder Parent { get; init; }
}