using System;
using InfraManager;

namespace Inframanager.DAL.ActiveDirectory.Import
{
    //todo:раскрыть после релиза
    // [ObjectClassMapping(ObjectClass.UIADConfiguration)]
    // [OperationIdMapping(ObjectAction.Insert, OperationId.UIADConfiguration_Insert)]
    // [OperationIdMapping(ObjectAction.Update, OperationId.UIADConfiguration_Update)]
    // [OperationIdMapping(ObjectAction.Delete, OperationId.UIADConfiguration_Delete)]
    // [OperationIdMapping(ObjectAction.ViewDetails, OperationId.None)]
    // [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationId.None)]
    public class UIADConfiguration
    {
        public Guid ID { get; init; }

        public string Name { get; init; }

        public bool ShowUsersInADTree { get; init; }

        public string Note { get; init; }
    }
}