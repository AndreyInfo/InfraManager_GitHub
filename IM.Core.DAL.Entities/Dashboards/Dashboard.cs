using System;
using Inframanager;

namespace InfraManager.DAL.Dashboards;

[ObjectClassMapping(ObjectClass.Dashboard)]
[OperationIdMapping(ObjectAction.Delete, OperationID.Dashboard_Delete)]
[OperationIdMapping(ObjectAction.Insert, OperationID.Dashboard_AddDevExpress)]
[OperationIdMapping(ObjectAction.Update, OperationID.Dashboard_Update)]
[OperationIdMapping(ObjectAction.ViewDetails, OperationID.Dashboard_Properties)]
[OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Dashboard_Properties)]
public class Dashboard
{
    public Guid ID { get; init; }
    
    public string Name { get; init; }
    
    public Guid? DashboardFolderID { get; init; }
    
    public virtual DashboardFolder Folder { get; init; }

    public ObjectClass ObjectClassID { get; init; }
    
    public byte[] RowVersion { get; init; }
    
    public static bool DbFuncDashboardTreeItemIsVisible(ObjectClass classID, Guid? id, Guid? userID) => throw new NotImplementedException();
}