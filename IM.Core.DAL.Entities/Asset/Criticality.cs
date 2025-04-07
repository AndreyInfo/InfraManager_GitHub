using System;
using Inframanager;

namespace InfraManager.DAL.Asset
{
    
    [OperationIdMapping(ObjectAction.Insert, OperationID.Criticality_Add)]
    [OperationIdMapping(ObjectAction.Update, OperationID.Criticality_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.Criticality_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.Criticality_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.Criticality_Properties)]
    public class Criticality : Catalog<Guid>
    {
        public Criticality() // TODO: КОгда выпилтся старая реализация справочника, тогда удалить наследование и закрыть конструктор
        {
        }

        public Criticality(string name)
        {
            ID = Guid.NewGuid(); // TODO: генерить на стороне СуБД + не использовать GUID в качестве ПК
            Name = name;
        }

        public byte[] RowVersion { get; set; }
    }
}
