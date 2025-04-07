using System;
using InfraManager;
using InfraManager.DAL.Asset;

namespace Inframanager.DAL.ProductCatalogue.AdapterReference
{
    [ObjectClassMapping(ObjectClass.AdapterModelReference)]
    [OperationIdMapping(ObjectAction.Insert, OperationID.None)]
    [OperationIdMapping(ObjectAction.Update, OperationID.None)]
    [OperationIdMapping(ObjectAction.Delete, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.None)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.None)]
    public class AdapterModelReference
    {
        public Guid AdapterModelID { get; init; }
        
        public virtual AdapterType Model { get; init; }

        public Guid ParentAdapterModelID { get; set; }
        
        public virtual AdapterType ParentModel { get; init; } 
    }
}