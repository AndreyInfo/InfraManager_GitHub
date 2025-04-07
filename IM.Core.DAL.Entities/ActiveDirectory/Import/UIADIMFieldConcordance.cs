using System;
using InfraManager;
using InfraManager.DAL.Import;

namespace Inframanager.DAL.ActiveDirectory.Import
{
    //todo:раскрыть после релиза
    // [ObjectClassMapping(ObjectClass.UIADIMFieldConcordance)]
    // [OperationIdMapping(ObjectAction.Insert, OperationId.UIADIMFieldConcordance_Insert)]
    // [OperationIdMapping(ObjectAction.Update, OperationId.UIADIMFieldConcordance_Update)]
    // [OperationIdMapping(ObjectAction.Delete, OperationId.UIADIMFieldConcordance_Delete)]
    // [OperationIdMapping(ObjectAction.ViewDetails, OperationId.None)]
    // [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationId.None)]
    public class UIADIMFieldConcordance : UIIMFieldConcordance
    {
        public Guid ConfigurationID { get; init; }

        public Guid ClassID { get; init; }

    }
}