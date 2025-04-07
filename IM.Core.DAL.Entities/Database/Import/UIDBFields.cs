using System;
using Inframanager;

namespace InfraManager.DAL.Database.Import
{
    [ObjectClassMapping(ObjectClass.UIDBFileds)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.UIDBFileds_Insert)]
    [OperationIdMapping(ObjectAction.Update, OperationID.UIDBFileds_Update)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.UIDBFileds_Delete)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.UIDBFileds_ViewDetails)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.UIDBFileds_ViewDetailsArray)]
    public class UIDBFields
    {
        public Guid ID { get; init; }

        public Guid ConfigurationID { get; init; }
        
        public virtual UIDBConfiguration Configuration { get; set; }
        
        public long FieldID { get; init; }

        public string Value { get; init; }
    }
}