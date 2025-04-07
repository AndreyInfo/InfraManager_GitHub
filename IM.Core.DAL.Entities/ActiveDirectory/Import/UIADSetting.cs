using System;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.Import;

namespace Inframanager.DAL.ActiveDirectory.Import
{
    //todo:раскрыть после релиза
    // [ObjectClassMapping(ObjectClass.UIADSetting)]
    // [OperationIdMapping(ObjectAction.Insert, OperationId.UIADSetting_Insert)]
    // [OperationIdMapping(ObjectAction.Update, OperationId.UIADSetting_Update)]
    // [OperationIdMapping(ObjectAction.Delete, OperationId.UIADSetting_Delete)]
    // [OperationIdMapping(ObjectAction.ViewDetails, OperationId.None)]
    // [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationId.None)]
    public class UIADSetting : UISettingBase, IMarkableForDelete
    {
        public Guid? ADConfigurationID { get; set; }
    }
}