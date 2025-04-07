using System;
using InfraManager;

namespace Inframanager.DAL.ActiveDirectory.Import
{
    //todo:раскрыть после релиза
    // [ObjectClassMapping(ObjectClass.UIADIMClassConcordance)]
    // [OperationIdMapping(ObjectAction.Insert, OperationId.UIADIMClassConcordance_Insert)]
    // [OperationIdMapping(ObjectAction.Update, OperationId.UIADIMClassConcordance_Update)]
    // [OperationIdMapping(ObjectAction.Delete, OperationId.UIADIMClassConcordance_Delete)]
    // [OperationIdMapping(ObjectAction.ViewDetails, OperationId.None)]
    // [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationId.None)]
    public class UIADIMClassConcordance
    {
        public Guid ConfigurationID { get; init; }

        public Guid ClassID { get; init; }

        public long IMClassID { get; init; }

        public virtual UIADClass Class { get; init; }
    }
}