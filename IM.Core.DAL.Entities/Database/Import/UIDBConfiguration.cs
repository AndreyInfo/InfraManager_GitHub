using System;
using Inframanager;

namespace InfraManager.DAL.Database.Import
{
    [ObjectClassMapping(ObjectClass.UIDBConfiguration)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.UIDBConfiguration_Insert)]
    [OperationIdMapping(ObjectAction.Update, OperationID.UIDBConfiguration_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.UIDBConfiguration_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.UIDBConfiguration_ViewDetails)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.UIDBConfiguration_ViewDetailsArray)]
    public class UIDBConfiguration
    {
        public Guid ID { get; init; }

        public string Name { get; init; }

        public string Note { get; init; }
        
        public string OrganizationTableName { get; init; }
        
        public string SubdivisionTableName { get; init; }
        
        public string UserTableName { get; init; }
    }
}