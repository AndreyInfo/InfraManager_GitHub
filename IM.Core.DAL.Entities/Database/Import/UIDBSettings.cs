using System;
using Inframanager;
using InfraManager.DAL.Import;

namespace InfraManager.DAL.Database.Import
{
    [ObjectClassMapping(ObjectClass.UIDBSettings)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.UIDBSettings_Insert)]
    [OperationIdMapping(ObjectAction.Update, OperationID.UIDBSettings_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.UIDBSettings_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.UIDBSettings_ViewDetails)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.UIDBSettings_ViewDetailsArray)]
    public class UIDBSettings:UISettingBase
    {
        public Guid ID { get; init; }
        
        public Guid? DBConfigurationID { get; init; }
        
        public string DatabaseName { get; init; }
        
    }
}