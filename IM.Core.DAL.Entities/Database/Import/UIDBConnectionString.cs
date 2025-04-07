using System;
using Inframanager;

namespace InfraManager.DAL.Database.Import
{
    [ObjectClassMapping(ObjectClass.UIDBConnectionString)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.UIDBConnectionString_Insert)]
    [OperationIdMapping(ObjectAction.Update, OperationID.UIDBConnectionString_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.UIDBConnectionString_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.UIDBConnectionString_ViewDetails)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.UIDBConnectionString_ViewDetailsArray)]
    public class UIDBConnectionString
    {
        public Guid ID { get; init; }

        public Guid? SettingsID { get; init; }

        public string ConnectionString { get; init; }
        
        public int? ImportSourceType { get; init; }
    }
}