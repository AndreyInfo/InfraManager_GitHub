using System;
using InfraManager;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;

namespace Inframanager.DAL.ProductCatalogue.Synonyms
{
    [ObjectClassMapping(ObjectClass.Synonym)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.None)]
    [OperationIdMapping(ObjectAction.Update, OperationID.None)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class Synonym
    {
        public Guid ID { get; set; }

        public ObjectClass ClassID { get; init; }

        public Guid? ModelID { get; set; }

        public string ModelName { get; init; }

        public string ModelProducer { get; init; }

        public virtual AdapterType AdapterType { get; init; }
        
        public virtual PeripheralType PeripheralType { get; init; }
    }
}